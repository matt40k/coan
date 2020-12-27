using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace COAN
{
    public class Config
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string appConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "OpenTTD", "coan");
        private readonly Poco.Config _config;

        private string GetAppConfig
        {
            get
            {
                logger.Log(LogLevel.Trace, "GetAppConfig");
                return Path.Combine(appConfigDir, "coan.json");
            }
        }

        private Poco.Config ReadConfig
        {
            get
            {
                logger.Log(LogLevel.Trace, "ReadConfig()");
                if (File.Exists(GetAppConfig))
                {
                    var configContent = File.ReadAllText(GetAppConfig);
                    if (!string.IsNullOrWhiteSpace(configContent))
                    {
                        logger.Log(LogLevel.Trace, "Config found");
                        return JsonConvert.DeserializeObject<Poco.Config>(configContent);
                    }
                }
                return null;
            }
        }

        public Config()
        {
            _config = ReadConfig;
        }

        public string GetDefaultHost
        {
            get
            {
                if (_config != null)
                {
                    var defaultHost = _config.Servers[0].Host;
                    if (!string.IsNullOrWhiteSpace(defaultHost))
                        return defaultHost;
                }
                return null;
            }
        }

        public int GetDefaultPort
        {
            get
            {
                if (_config != null)
                {
                    var defaultPort = _config.Servers[0].Port;
                    if (defaultPort.HasValue)
                        return defaultPort.Value;
                }
                return 3977;
            }
        }

        public string GetDefaultPassword
        {
            get
            {
                if (_config != null)
                {
                    var defaultPassword = _config.Servers[0].Password;
                    if (!string.IsNullOrWhiteSpace(defaultPassword))
                        return defaultPassword;
                }
                return null;
            }
        }
    }
}
