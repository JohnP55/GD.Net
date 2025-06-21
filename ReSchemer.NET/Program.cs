using GD;
using GD.Level;

namespace ReSchemer.NET
{
    internal class Program
    {
        static void SwapRedGreen(GDLevel level)
        {
            foreach (var color in level.Header.Colors)
            {
                (color["FromColourRed"], color["FromColourGreen"]) = (color["FromColourGreen"], color["FromColourRed"]);
            }
            foreach (GDLevelObject obj in level.Objects)
            {
                if (obj.ContainsKey("red") && obj.ContainsKey("green") && obj.ContainsKey("blue"))
                {
                    (obj["red"], obj["green"]) = (obj["green"], obj["red"]);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Level ID to rescheme: ");
            if (!int.TryParse(Console.ReadLine(), out int levelId))
            {
                Console.WriteLine("Invalid level ID.");
                return;
            }

            GDLevel level;
            try
            {
                level = API.DownloadLevel(levelId);
            }
            catch (LevelDoesNotExistException)
            {
                Console.WriteLine("Level not found.");
                return;
            }

            SwapRedGreen(level);

            Console.WriteLine("Level reschemed! It will be reuploaded to the servers.");
            Console.Write("GD username: ");
            string username = Console.ReadLine()!;
            Console.Write("GD password: ");
            string password = Console.ReadLine()!;

            Console.Write("What will you name the level? ");
            level.Name = Console.ReadLine()!;

            Console.Write("Should it be uploaded unlisted? (Y/n) ");
            level.IsUnlisted = Console.ReadLine() != "n";

            GDAccount account = new GDAccount(username, password);
            Console.WriteLine($"Level uploaded! ID is {API.UploadLevel(account, level)}.");
        }
    }
}
