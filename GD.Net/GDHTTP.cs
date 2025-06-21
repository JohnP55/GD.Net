using System.Text;

namespace GD
{
    internal static class GDHTTP
    {
        private static HttpClient GetHttpClient(string baseAddress = DEFAULT_URL)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "");
            return client;
        }
        public const string DEFAULT_SECRET = "Wmfd2893gb7";
        public const string ACCOUNT_SECRET = "Wmfv3899gc9";
        public const string LEVEL_DELETION_SECRET = "Wmfv2898gc9";
        public const string MOD_SECRET = "Wmfp3879gc3";

        public const string DEFAULT_URL = "http://www.boomlings.com/database/";
        public const string SAVEDATA_URL = "http://www.robtopgames.net/database/";
        public static string Get(string url)
        {
            using (var client = GetHttpClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }
        public static GDServerResponse Post(string url, string parameters, string secret=DEFAULT_SECRET, char separator=GDServerResponse.DEFAULT_SEPARATOR, string baseAddress = DEFAULT_URL, string gameVersion = "22", string binaryVersion = "38")
        {
            StringContent p = new StringContent($"gameVersion={gameVersion}&binaryVersion={binaryVersion}&gdw=0&{parameters}&secret={secret}", Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var client = GetHttpClient(baseAddress))
            {
                return new GDServerResponse(client.PostAsync(url, p).Result.Content.ReadAsStringAsync().Result, separator);
            }
        }
    }
}
