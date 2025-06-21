using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD
{
    // TODO: GJP2 (I'm releasing this before adding anything new, this has been sitting on my pc for like 2 years)
    public static class GJPTools
    {
        private const string KEY = "37526";
        public static string Encode(string data) => Utils.XorBase64(data, KEY);
        public static string Decode(string gjp) => Utils.UndoXorBase64(gjp, KEY);
            
    }
}
