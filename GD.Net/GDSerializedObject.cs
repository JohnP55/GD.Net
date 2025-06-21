using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD
{
    /// <summary>
    /// Object does not contain property
    /// </summary>
    public class PropertyNotUsedException : Exception
    {
        public PropertyNotUsedException() : base("Tried to access property in an object that did not contain it.") { }
    }

    public class GDSerializedObject
    {
        protected class EvenSeparatorCountException : Exception
        {
            public EvenSeparatorCountException() : base("The response cannot be evenly separated in key-value pairs (The number of separators is even)") { }
        }
        protected char Separator { get; }
        private const char EXTRA_DATA_SEPARATOR = '#';
        public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();

        public GDSerializedObject(char separator) : this(null, separator) { }

        public GDSerializedObject(string? data, char separator)
        {
            Separator = separator;
            
            if (data is null)
                return;

            if (Separator != EXTRA_DATA_SEPARATOR)
            {
                data = data.Split(EXTRA_DATA_SEPARATOR)[0];
            }
            
            if (!data.Any(x => x == Separator))
            {
                Data.Add("0", data);
                return;
            }
            
            if (data.Count(x => x == Separator) % 2 == 0)
                throw new EvenSeparatorCountException();

            string[] attrs = data.Split(Separator);

            for (int i = 0; i < attrs.Length; i += 2)
            {
                Data.Add(attrs[i], attrs[i + 1]);
            }
        }
        public GDSerializedObject(GDSerializedObject gdso)
        {
            Data = gdso.Data;
            Separator = gdso.Separator;
        }

        public virtual string this[string s]
        {
            get
            {
                if (!Data.TryGetValue(s, out string? propValue))
                    throw new PropertyNotUsedException();

                return propValue;
            }
            set
            {
                Data[s] = value;
            }
        }
        public virtual bool ContainsKey(string key)
        {
            return Data.ContainsKey(key);
        }
        public virtual bool TryGetValue(string s, out string? val)
        {
            if (ContainsKey(s))
            {
                val = this[s];
                return true;
            }
            else
            {
                val = null;
                return false;
            }
        }
        public string GetString<T>(T key)
        {
            Data.TryGetValue(key!.ToString()!, out string? result);
            return result ?? "";
        }
        public int GetInt<T>(T key)
        {
            if (Data.TryGetValue(key!.ToString()!, out string? result))
                if (result != "")
                    return Convert.ToInt32(result);
            return 0;
        }
        public bool GetBoolean<T>(T key)
        {
            if (Data.TryGetValue(key!.ToString()!, out string? result))
                if (result != "")
                    return result == "1";
            return false;
        }
        public override string ToString()
        {
            return string.Join(Separator, Data.ToList());
        }
    }
}
