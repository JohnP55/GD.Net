using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(@"Z:\PS3 Emulation\Games\FNC\BLUS30608\BLUS-30608 - Copy\PS3_GAME\USRDIR\data\common\fmv\mm_20-32.vp6");
            string contents = Utils.GzipBase64(sr.ReadToEnd());
            sr.Close();

            GDAccount auth = new GDAccount("GDJohnP55", "");
            GDLevel level = new GDLevel("testlevel", contents, 5555, true, "this is a test don't @ me", isUnlisted: true);

            Console.WriteLine("uploading the level");
            Console.WriteLine(Methods.UploadLevel(auth, level));
        }
    }
}
