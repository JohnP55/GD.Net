using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPI
{
    public static class GJPTools
    {
        private const string KEY = "37526";
        public static string Encode(string data) => Utils.XorBase64(data, KEY);
        public static string Decode(string gjp) => Utils.UndoXorBase64(gjp, KEY);
            
    }
}
