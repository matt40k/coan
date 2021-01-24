namespace org.openttd
{
    public enum AdminUpdateFrequency
    {
        /// <summary>
        /// This is manual request
        /// </summary>
        ADMIN_FREQUENCY_POLL = 0x01,
        /// <summary>
        /// This is every game day
        /// </summary>
        ADMIN_FREQUENCY_DAILY = 0x02,
        /// <summary>
        /// This updates us every game week
        /// </summary>
        ADMIN_FREQUENCY_WEEKLY = 0x04,
        /// <summary>
        /// This updates us every game month
        /// </summary>
        ADMIN_FREQUENCY_MONTHLY = 0x08,
        /// <summary>
        /// This updates us every 3 game months
        /// </summary>
        ADMIN_FREQUENCY_QUARTERLY = 0x10,
        /// <summary>
        /// This updates us once a game year
        /// </summary>
        ADMIN_FREQUENCY_ANUALLY = 0x20,
        /// <summary>
        /// This updates us as and when something happens.
        /// </summary>
        ADMIN_FREQUENCY_AUTOMATIC = 0x40
    }
    public enum AdminUpdateType
    {
        ADMIN_UPDATE_DATE            = 0,
        /// <summary>
        /// This is any information regarding clients: JOIN, QUIT, UPDATE, ERROR
        /// </summary>
        ADMIN_UPDATE_CLIENT_INFO = 1,
        ADMIN_UPDATE_COMPANY_INFO    = 2,
        ADMIN_UPDATE_COMPANY_ECONOMY = 3,
        ADMIN_UPDATE_COMPANY_STATS   = 4,
        ADMIN_UPDATE_CHAT            = 5,
        ADMIN_UPDATE_CONSOLE         = 6,
        ADMIN_UPDATE_CMD_NAMES       = 7,
        ADMIN_UPDATE_CMD_LOGGING     = 8,
        ADMIN_UPDATE_GAMESCRIPT      = 9,
        ADMIN_UPDATE_END             = 10
    }
}
