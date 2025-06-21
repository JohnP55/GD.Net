using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GD.Level;

namespace GD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllBytes("shrek.mp4", API.DownloadLevel(90943078, shouldParseAsRawData: true).DecompressedLevelData!);
        }
    }
}
