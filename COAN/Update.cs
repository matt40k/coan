using System;
using System.Net.Http;
using System.Net.Http.Headers;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace COAN
{
    public class Update
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string repo;
        private readonly string user;

        public Update(string GitHubUser, string GitHubRepository)
        {
            user = GitHubUser;
            repo = GitHubRepository;
        }

        /// <summary>
        /// Returns local application version
        /// </summary>
        public Version GetLocalVersion
        {
            get
            {
                var result = Version.TryParse(Info.Version, out Version local);
                logger.Log(LogLevel.Trace, string.Format("GetLocalVersion - {0}", Info.Version));
                if (result)
                    return local;
                return null;
            }
        }

        /// <summary>
        /// Returns latest version
        /// 
        /// Uses the GitHub API to get the latest version of the application
        /// </summary>
        public string GetServerVersionString
        {
            get
            {
                try
                {
                    var client = new HttpClient
                    {
                        BaseAddress = new Uri("https://api.github.com")
                    };
                    client.DefaultRequestHeaders.Add("User-Agent", Info.Title);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var url = string.Format("repos/{0}/{1}/releases/latest", user, repo); 
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    var resp = response.Content.ReadAsStringAsync().Result;
                    logger.Log(LogLevel.Trace, string.Format("GetServerVersionString - GitHub API response - {0}", resp));

                    JObject respJson = JObject.Parse(resp);
                    DownloadUrl = (string)respJson["assets"][0]["browser_download_url"];
                    logger.Log(LogLevel.Trace, string.Format("GetServerVersionString - Download Url - {0}", DownloadUrl));
                    var serverVersion = (string)respJson["tag_name"];
                    logger.Log(LogLevel.Trace, string.Format("GetServerVersionString - Version - {0}", serverVersion));
                 
                    return serverVersion;
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Trace, string.Format("{0}", ex));
                }
                return null;
            }
        }

        /// <summary>
        /// Returns latest version
        /// </summary>
        public Version GetServerVersion
        {
            get
            {
                var result = Version.TryParse(GetServerVersionString, out Version remote);
                if (result)
                    return remote;
                return null;
            }
        }

        /// <summary>
        /// Returns a url to download the latest version of the application if the local version isn't up-to-date.
        /// </summary>
        public string DownloadUrl { get; private set; }

        public bool IsOutofdate
        {
            get
            {
                bool result = ((GetLocalVersion.CompareTo(GetServerVersion)) < 0);
                logger.Log(LogLevel.Trace, string.Format("IsOutofdate {0}", result));
                return result;
            }
        }
    }
}
