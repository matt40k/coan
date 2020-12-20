namespace COAN
{
    public enum AdminUpdateFrequency
    {
        ADMIN_FREQUENCY_POLL      = 0x01, //This is manual request
        ADMIN_FREQUENCY_DAILY     = 0x02, //This is every game day
        ADMIN_FREQUENCY_WEEKLY    = 0x04, //This updates us every game week
        ADMIN_FREQUENCY_MONTHLY   = 0x08, //This updates us every game month
        ADMIN_FREQUENCY_QUARTERLY = 0x10, //This updates us every 3 game months
        ADMIN_FREQUENCY_ANUALLY   = 0x20, //This updates us once a game year
        ADMIN_FREQUENCY_AUTOMATIC = 0x40 //This updates us as and when something happens.
    }
    public enum AdminUpdateType
    {
        ADMIN_UPDATE_DATE            = 0, 
        ADMIN_UPDATE_CLIENT_INFO     = 1, //This is any information regarding clients: JOIN, QUIT, UPDATE, ERROR
        ADMIN_UPDATE_COMPANY_INFO    = 2, //
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
