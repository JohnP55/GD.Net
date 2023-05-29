using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GDAPI
{
    public class Cycle
    {
        private readonly string _s;
        public char this[int index] => _s[index % _s.Length];
        public Cycle(string s)
        {
            _s = s;
        }
        public override string ToString() => _s;
    }
    public static class Utils
    {
        private static readonly char padding = '=';
        public static string ToBase64StringUrlSafe(byte[] inArray)
        {
            return Convert.ToBase64String(inArray).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }
        public static byte[] FromBase64StringUrlSafe(string s)
        {
            s = s.Replace('_', '/').Replace('-', '+');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
                default: break;
            }
            return Convert.FromBase64String(s);
        }
        public static byte[] ToByteArray(this string str) => Encoding.ASCII.GetBytes(str);
        public static string ToStr(this byte[] bytes) => Encoding.ASCII.GetString(bytes);
        public static string Base64Encode(this string str) => ToBase64StringUrlSafe(str.ToByteArray());
        public static string Base64Decode(this string str) => FromBase64StringUrlSafe(str).ToStr();
        public static byte[] Xor(byte[] data, string key)
        {
            var result = new byte[data.Length];
            Cycle cycleKey = new Cycle(key);

            for (int i = 0; i < data.Length; i++)
                result[i] = (byte)(data[i] ^ cycleKey[i]);

            return result;
        }
        public static string XorBase64(string data, string key)
        {
            return XorBase64(data.ToByteArray(), key);
        }
        public static string XorBase64(byte[] data, string key)
        {
            return ToBase64StringUrlSafe(Xor(data, key));
        }
        public static byte[] UndoXorBase64_bytes(string data, string key)
        {
            return Xor(FromBase64StringUrlSafe(data), key);
        }
        public static string UndoXorBase64(string data, string key)
        {
            return Encoding.ASCII.GetString(UndoXorBase64_bytes(data, key));
        }
        public static string GzipBase64(string data)
        {
            using var resultStream = new MemoryStream();
            using var zipStream = new GZipStream(resultStream, CompressionMode.Compress);
            zipStream.Write(data.ToByteArray(), 0, data.Length);
            return ToBase64StringUrlSafe(resultStream.ToArray());
        }
    }
}
