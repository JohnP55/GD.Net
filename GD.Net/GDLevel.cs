using Newtonsoft.Json;

namespace GD.Level
{
    /// <summary>
    /// The file containing data on object properties was not found.
    /// </summary>
    public class PropertiesJsonNotFoundException : Exception
    {
        public PropertiesJsonNotFoundException(string filename) : base($"The file {filename} was not found.") { }
    }
    /// <summary>
    /// The file containing data on object properties is invalid.
    /// </summary>
    public class PropertiesJsonInvalidException : Exception
    {
        public PropertiesJsonInvalidException(string filename) : base($"The file {filename} does not contain valid data.") { }
    }
    /// <summary>
    /// Property does not exist in objprops.json
    /// </summary>
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() : base("Tried to access property that does not exist.") { }
    }

    public class GDLevelAttribute : GDSerializedObject
    {
        protected static Dictionary<string, string> InitializePropsJson(string filename)
        {
            string data;
            try
            {
                data = File.ReadAllText(filename);
            }
            catch (IOException)
            {
                throw new PropertiesJsonNotFoundException(filename);
            }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(data) ?? throw new PropertiesJsonInvalidException(filename);
        }
        public Dictionary<string, string> Properties { get; }
        public override string this[string s]
        {
            get
            {
                if (!Properties.TryGetValue(s, out string? propKey))
                    throw new PropertyNotFoundException();
                return base[propKey];
            }
            set
            {
                if (!Properties.TryGetValue(s, out string? propKey))
                    throw new PropertyNotFoundException();
                base[propKey] = value;
            }
        }
        public override bool ContainsKey(string key)
        {
            if (!Properties.TryGetValue(key, out string? propKey))
                throw new PropertyNotFoundException();
            return base.ContainsKey(propKey);
        }
        protected GDLevelAttribute(char separator, Dictionary<string, string> properties) : this(null, separator, properties) { }
        protected GDLevelAttribute(string? data, char separator, Dictionary<string, string> properties) : base(data, separator)
        {
            Properties = properties;
        }
    }
    public class GDLevelObject : GDLevelAttribute
    {
        public const char DEFAULT_SEPARATOR = ',';
        public new static Dictionary<string, string> Properties { get; } = InitializePropsJson("../../../../objprops.json");
        public GDLevelObject() : this(null) { }
        public GDLevelObject(string? data) : base(data, DEFAULT_SEPARATOR, Properties) { }
    }
    public class GDLevelColor : GDLevelAttribute
    {
        public const char DEFAULT_SEPARATOR = '_';
        public new static Dictionary<string, string> Properties { get; } = InitializePropsJson("../../../../colorprops.json");
        public GDLevelColor() : this(null) { }
        public GDLevelColor(string? data) : base(data, DEFAULT_SEPARATOR, Properties) { }
    }
    public class GDLevelHeader : GDLevelAttribute
    {
        public const char DEFAULT_SEPARATOR = ',';
        public const char COLOR_SEPARATOR = '|';
        public List<GDLevelColor> Colors { get; } = new();
        public new static Dictionary<string, string> Properties { get; } = InitializePropsJson("../../../../headerprops.json");
        public GDLevelHeader() : this(null) { }
        public GDLevelHeader(string? data) : base(data, DEFAULT_SEPARATOR, Properties)
        {
            var colorStrs = this["Colors"].Split(COLOR_SEPARATOR).ToList();
            colorStrs.RemoveAt(colorStrs.Count - 1); // | is an end tag but it's infinitely easier to use as a separator
            var colors = colorStrs.Select(e => new GDLevelColor(e));
            foreach (GDLevelColor color in colors)
                Colors.Add(color);
        }
        public override string ToString()
        {
            this["Colors"] = string.Join(COLOR_SEPARATOR, Colors) + COLOR_SEPARATOR;
            return base.ToString();
        }
    }
    public class GDLevel
    {
        private const char LEVEL_SEPARATOR = ';';
        public GDLevelHeader Header { get; }
        public List<GDLevelObject> Objects { get; }

        public int Id { get; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public int SongId { get; set; }
        public bool IsNewgrounds { get; set; }

        // 300 chars max
        public string ExtraString { get; set; }
        public bool IsUnlisted { get; set; }
        private string levelStringOnLoad;
        private byte[]? decompressedLevelDataOnLoad;
        public string LevelString => ShouldParseAsRawData ? levelStringOnLoad : ComputeLevelStringAndCompress();
        public byte[]? DecompressedLevelData => ShouldParseAsRawData ? decompressedLevelDataOnLoad : ComputeLevelString().ToByteArray();
        public bool ShouldParseAsRawData { get; set; }

        public GDLevel(GDLevelResponse data, bool shouldParseAsRawData = false, bool dontTryDecompressing = false) : this(data.Name, data.LevelString, data.CustomSongId == 0 ? data.OfficialSong : data.CustomSongId, data.CustomSongId != 0, data.Description, data.ExtraString, false, shouldParseAsRawData, dontTryDecompressing)
        {
            Id = data.Id;
        }
        public GDLevel(string name, string levelString, int songId, bool isNewgrounds, string description = "", string extraString = "", bool isUnlisted = false, bool shouldParseAsRawData = false, bool dontTryDecompressing = false)
        {
            Name = name;
            Description = description;
            levelStringOnLoad = levelString;
            SongId = songId;
            IsNewgrounds = isNewgrounds;
            ExtraString = extraString;
            IsUnlisted = isUnlisted;
            ShouldParseAsRawData = shouldParseAsRawData;

            if (dontTryDecompressing)
            {
                decompressedLevelDataOnLoad = levelStringOnLoad.ToByteArray();
            }
            else
            {
                decompressedLevelDataOnLoad = levelStringOnLoad[..2] switch
                {
                    "H4" => Utils.UndoGzipBase64(levelStringOnLoad),
                    "eJ" => Utils.UndoZlibBase64(levelStringOnLoad),
                    _ => levelStringOnLoad.ToByteArray()
                };
            }

            Objects = new List<GDLevelObject>();
            if (!shouldParseAsRawData)
            {
                var objects = decompressedLevelDataOnLoad.ToStr().Split(LEVEL_SEPARATOR).ToList(); // DecompressedLevelData is a byte array in case the user downloads something other than a level. Level data is always a string, so if shouldParseAsRawData is false then we know DecompressedLevelData has to be parseable as a string
                objects.RemoveAt(objects.Count - 1); // Technically level objects end with ; but I use it as a separator because i'm slightly less cringe than robtop
                Header = new GDLevelHeader(objects[0]);
                objects.RemoveAt(0);

                foreach (var data in objects)
                {
                    Objects.Add(new GDLevelObject(data));
                }
            } else
            {
                Header = new GDLevelHeader();
            }
        }
        public string ComputeLevelString()
        {
            return Header.ToString() + LEVEL_SEPARATOR + string.Join(LEVEL_SEPARATOR, Objects) + LEVEL_SEPARATOR;
        }
        public string ComputeLevelStringAndCompress()
        {
            return Utils.GzipBase64(ComputeLevelString());
        }
    }
}
