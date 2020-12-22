using System.Collections.Generic;

namespace COAN.Poco
{
    public class Config
    {
        public int DefaultServerId { get; set; }
        public IList<Server> Servers { get; set; }
    }

    public class Server
    {
        public int ServerId { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
    }
}
