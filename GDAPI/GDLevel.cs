using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Newtonsoft.Json;

namespace GDAPI.Level
{
    public class ObjectPropertiesNotFoundException : Exception
    {
        public ObjectPropertiesNotFoundException() : base("objprops.json was not found.") { }
    }
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() : base("Tried to access property that does not exist.") { }
    }
    public class PropertyNotUsedException : Exception
    {
        public PropertyNotUsedException() : base("Tried to access property in an object that did not contain it.") { }
    }

    // TODO asf, not really my problem for what i'm doing (this will be future me's problem)
    // oh no i'm future me
    public class LevelObject : GDSerializedObject
    {
        private const char OBJ_SEPARATOR = ',';
        private static readonly Dictionary<string, int> Properties = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText("objprops.json")) ?? throw new ObjectPropertiesNotFoundException();

        public string this[string s]
        {
            get
            {
                if (!Properties.TryGetValue(s, out int attrKey))
                    throw new PropertyNotFoundException();
                if (!Data.TryGetValue(attrKey, out string? attrValue))
                    throw new PropertyNotUsedException();

                return attrValue;
            }
            set
            {
                if (!Properties.TryGetValue(s, out int attrKey))
                    throw new PropertyNotFoundException();
                Data[attrKey] = value;
            }
        }

        public LevelObject() : base(OBJ_SEPARATOR) { }
        public LevelObject(string data) : base(data, OBJ_SEPARATOR)
        {

        }
    }
    public class Header
    {

    }
    public class GDLevel
    {
        //public GDLevelHeader Header { get; }
        //public List<Object> Objects { get; }

        public int Id { get; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public int SongId { get; set; }
        public bool IsNewgrounds { get; set; }

        // 300 chars max
        public string ExtraString { get; set; }
        public bool IsUnlisted { get; set; }
        public string LevelString { get; }
        public bool IsGzip => LevelString.StartsWith("H4s");
        public byte[] LevelData
        {
            get
            {
                using var compressedStream = new MemoryStream(Utils.FromBase64StringUrlSafe(LevelString));
                using var resultStream = new MemoryStream();
                if (IsGzip)
                {
                    using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
                    zipStream.CopyTo(resultStream);
                }
                else
                {
                    using var zlibStream = new ZLibStream(compressedStream, CompressionMode.Decompress);
                    zlibStream.CopyTo(resultStream);
                }
                return resultStream.ToArray();
            }
        }

        public GDLevel(GDLevelResponse levelResponse)
        {
            //Header = new GDLevelHeader();
            //Objects = new List<Object>();

            Id = levelResponse.Id;
            Name = levelResponse.Name;
            Description = levelResponse.Description;

            if (levelResponse.CustomSongId == 0)
            {
                SongId = levelResponse.OfficialSong;
                IsNewgrounds = false;
            }
            else
            {
                SongId = levelResponse.CustomSongId;
                IsNewgrounds = true;
            }
            LevelString = levelResponse.LevelString;
            ExtraString = levelResponse.ExtraString;
        }
        public GDLevel(string name, string levelString, int songId, bool isNewgrounds, string description = "", string extraString = "", bool isUnlisted = false)
        {
            Name = name;
            Description = description;
            LevelString = levelString;
            SongId = songId;
            IsNewgrounds = isNewgrounds;
            ExtraString = extraString;
            IsUnlisted = isUnlisted;
        }
    }
}
