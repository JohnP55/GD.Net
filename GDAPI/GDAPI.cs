using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace GDAPI
{
    public static class Methods
    {
        public static bool PostComment(GDAccount account, int levelId, string comment, int percentage)
        {
            string url = "uploadGJComment21.php";
            string encodedComment = comment.Base64Encode();
            string parameters = $"accountID={account.AccountID}&gjp={account.GJP}&userName={account.Username}&comment={encodedComment}&levelID={levelId}&percent={percentage}&chk={CHK.COMMENT_CHK.Make(account.Username, encodedComment, levelId.ToString(), percentage.ToString(), "0")}";
            return GDHTTP.Post(url, parameters).Status == Status.Success;
        }

        public static string GetSaveData(GDAccount account)
        {
            string url = "accounts/syncGJAccountNew.php";
            string parameters = $"userName={account.Username}&password={account.Password}";
            return GDHTTP.Post(url, parameters, secret:GDHTTP.ACCOUNT_SECRET, baseAddress: GDHTTP.SAVEDATA_URL).Data[0].Split(';')[0];
        }
        public static bool BackupSaveData(GDAccount account, string savedata)
        {
            string url = "accounts/backupGJAccountNew.php";
            string parameters = $"userName={account.Username}&password={account.Password}&saveData={savedata}";
            return GDHTTP.Post(url, parameters, secret: GDHTTP.ACCOUNT_SECRET, baseAddress: GDHTTP.SAVEDATA_URL).Status == Status.Success;
        }

        public static int UploadLevel(GDAccount account, GDLevel level)
        {
            return UploadLevel(account, level.Name, level.Description, level.SongId, level.IsNewgrounds, level.LevelString, level.ExtraString, level.IsUnlisted);
        }
        public static int UploadLevel(GDAccount account, string name, string description, int songId, bool isNewgrounds, string levelString, string extraString, bool isUnlisted)
        {
            StringBuilder seed2Sb = new();

            if (levelString.Length < 50)
                seed2Sb.Append(levelString);
            else
            {
                for (int i = 0; i < 50; i++)
                    seed2Sb.Append(levelString[levelString.Length / 50 * i]);
            }

            string seed2 = CHK.LEVEL_CHK.Make(seed2Sb.ToString());

            string url = "uploadGJLevel21.php";
            string parameters = $"accountID={account.AccountID}&gjp={account.GJP}&userName={account.Username}&levelID=0&levelName={name}&levelDesc={description.Base64Encode()}&levelVersion=0&levelLength=0&audioTrack={(isNewgrounds ? 0 : songId)}&auto=0&password=1&original=0&twoPlayer=0&songID={(isNewgrounds ? songId : 0)}&objects=33000&coins=0&requestedStars=0&unlisted={(isUnlisted ? 1 : 0)}&wt=24&wt2=0&extraString={extraString}&seed=gUmgKRJgsU&seed2={seed2}&levelString={levelString}&levelInfo=H4sIAAAAAAAACzPUMzW1NrAGAB3_mUYHAAAA";
            return new GDIntegerResponse(GDHTTP.Post(url, parameters)).Result;
        }
        public static GDLevelResponse GetLevelMetadata(int levelId)
        {
            string url = "getGJLevels21.php";
            string parameters = $"str={levelId}";
            return new GDLevelResponse(GDHTTP.Post(url, parameters));
        }
        public static GDLevel DownloadLevel(int levelId)
        {
            string url = "downloadGJLevel22.php";
            string parameters = $"levelID={levelId}&inc=0&extras=0";
            return new GDLevel(new GDLevelResponse(GDHTTP.Post(url, parameters)));
        }
        public static bool DeleteLevel(GDAccount account, int levelId)
        {
            string url = "deleteGJLevelUser20.php";
            string parameters = $"accountID={account.AccountID}&gjp={account.GJP}&levelID={levelId}";
            return GDHTTP.Post(url, parameters, secret: GDHTTP.LEVEL_DELETION_SECRET).Status == Status.Success;
        }
    }
}