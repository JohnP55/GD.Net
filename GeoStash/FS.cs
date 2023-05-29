namespace GeoStash
{
    public class Folder
    {
        public List<Folder> Subfolders { get; }
        public List<File> Files { get; }
        public Folder()
        {
            Subfolders = new List<Folder>();
            Files = new List<File>();
        }
    }
    public class File
    {
        public string Name { get; set; }
        public int LevelId { get; }
        public byte[] Download()
        {
            return GDAPI.Methods.DownloadLevel(LevelId).LevelData;
        }

        public File(int levelId)
        {

        }
    }
}
