using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPI
{
    // TODO: Implement this with server responses
    public class GDSerializedObject
    {
        protected class EvenSeparatorCountException : Exception
        {
            public EvenSeparatorCountException() : base("The response cannot be evenly separated in key-value pairs (The number of separators is even)") { }
        }
        protected char Separator { get; }
        public Dictionary<int, string> Data { get; } = new Dictionary<int, string>();

        public GDSerializedObject(char separator)
        {
            Separator = separator;
        }

        public GDSerializedObject(string data, char separator)
        {
            Separator = separator;

            if (data.Count(x => x == Separator) % 2 == 0)
                throw new EvenSeparatorCountException();

            string[] attrs = data.Split(Separator);

            for (int i = 0; i < attrs.Length; i += 2)
            {
                Data.Add(Convert.ToInt32(attrs[i]), attrs[i + 1]);
            }
        }
    }
}
