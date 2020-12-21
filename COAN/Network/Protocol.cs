using System.Collections.Generic;
using System.Linq;
using System.Collections;
using NLog;

namespace COAN
{
    public class Protocol
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        internal int version = -1;
        protected Dictionary<AdminUpdateType, ArrayList> supportedFrequencies;

        public Protocol()
        {
            supportedFrequencies = new Dictionary<AdminUpdateType, ArrayList>();
        }

        public void addSupport(int typeIndex, int freqIndex)
        {
            AdminUpdateFrequency freq = (AdminUpdateFrequency)freqIndex;
            AdminUpdateType type = (AdminUpdateType)typeIndex;

            if (supportedFrequencies.Keys.Contains(type) == false)
            {
                supportedFrequencies.Add(type, new ArrayList());
            }
            supportedFrequencies[type].Add(freq);

        }

        public bool isSupported(AdminUpdateType type, AdminUpdateFrequency freq)
        {
            return supportedFrequencies[type].Contains(freq);
        }

        public int getVersion()
        {
            return this.version;
        }
    }
}
