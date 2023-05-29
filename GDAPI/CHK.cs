using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GDAPI
{
    public struct CHK
    {
        public static readonly CHK COMMENT_CHK = new CHK("29481", "xPT6iUrtws0J");
        public static readonly CHK LEVEL_CHK = new CHK("41274", "xI25fpAapCQg");
        public static readonly CHK STAT_CHK = new CHK("58281", "xI35fsAapCRg");
        public static readonly CHK LIKE_CHK = new CHK("85271", "ysg6pUrtjn0J");
        public CHK(string key, string salt)
        {
            Key = key;
            Salt = salt;
        }
        public string Key { get; }
        public string Salt { get; }
        public string Make(params string[] strings)
        {
            byte[] full = Encoding.ASCII.GetBytes(new StringBuilder(string.Concat(strings)).Append(Salt).ToString());

            // Robtop specifically converts the sha1 bytearray to lowercase string before XORing. Why.
            string hashed = Convert.ToHexString(SHA1.HashData(full)).ToLower();
            return Utils.XorBase64(hashed, Key);
        }
    }
}
