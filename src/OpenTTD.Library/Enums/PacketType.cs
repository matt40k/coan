namespace org.openttd
{
    public enum PacketType
    {   ///<summary> The admin announces and authenticates itself to the</summary> 
        ADMIN_PACKET_ADMIN_JOIN                 = 0,
        ///<summary> The admin tells the server that it is quitting</summary>
        ADMIN_PACKET_ADMIN_QUIT                 = 1,    
        ///<summary> The admin tells the server the update frequency of a particular piece of information.</summary>
        ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY     = 2,   
        ///<summary> The admin explicitly polls for a piece of information.</summary>
        ADMIN_PACKET_ADMIN_POLL                 = 3,
        ///<summary> The admin sends a chat message to be distributed.</summary>
        ADMIN_PACKET_ADMIN_CHAT                 = 4,
        ///<summary> The admin sends a remote console command.</summary>
        ADMIN_PACKET_ADMIN_RCON                 = 5,
        ///<summary> The admin sends a JSON string for the GameScript.</summary>
        ADMIN_PACKET_ADMIN_GAMESCRIPT           = 6,
        ///<summary> The admin sends a ping to the server, expecting a ping-reply (PONG, packet.</summary>
        ADMIN_PACKET_ADMIN_PING                 = 7,
        /// <summary>
        /// The server tells the admin it cannot accept the admin.
        /// </summary>
        ADMIN_PACKET_SERVER_FULL = 100,
        /// <summary>
        /// The server tells the admin it is banned.
        /// </summary>
        ADMIN_PACKET_SERVER_BANNED = 101,
        /// <summary>
        /// The server tells the admin an error has occurred.
        /// </summary>
        ADMIN_PACKET_SERVER_ERROR = 102,
        /// <summary>
        /// The server tells the admin its protocol version.
        /// </summary>
        ADMIN_PACKET_SERVER_PROTOCOL = 103,
        /// <summary>
        /// The server welcomes the admin to a game.
        /// </summary>
        ADMIN_PACKET_SERVER_WELCOME = 104,
        /// <summary>
        /// The server tells the admin its going to start a new game.
        /// </summary>
        ADMIN_PACKET_SERVER_NEWGAME = 105,
        /// <summary>
        /// The server tells the admin its shutting down.
        /// </summary>
        ADMIN_PACKET_SERVER_SHUTDOWN = 106,
        /// <summary>
        /// The server tells the admin what the current game date is.
        /// </summary>
        ADMIN_PACKET_SERVER_DATE = 107,
        /// <summary>
        /// The server tells the admin that a client has joined.
        /// </summary>
        ADMIN_PACKET_SERVER_CLIENT_JOIN = 108,
        /// <summary>
        /// The server gives the admin information about a client.
        /// </summary>
        ADMIN_PACKET_SERVER_CLIENT_INFO = 109,
        /// <summary>
        /// The server gives the admin an information update on a client.
        /// </summary>
        ADMIN_PACKET_SERVER_CLIENT_UPDATE = 110,
        /// <summary>
        /// The server tells the admin that a client quit.
        /// </summary>
        ADMIN_PACKET_SERVER_CLIENT_QUIT = 111,
        /// <summary>
        /// The server tells the admin that a client caused an error.
        /// </summary>
        ADMIN_PACKET_SERVER_CLIENT_ERROR = 112,
        /// <summary>
        /// The server tells the admin that a new company has started.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_NEW = 113,
        /// <summary>
        /// The server gives the admin information about a company.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_INFO = 114,
        /// <summary>
        /// The server gives the admin an information update on a company.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_UPDATE = 115,
        /// <summary>
        /// The server tells the admin that a company was removed.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_REMOVE = 116,
        /// <summary>
        /// The server gives the admin some economy related company information.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_ECONOMY = 117,
        /// <summary>
        /// The server gives the admin some statistics about a company.
        /// </summary>
        ADMIN_PACKET_SERVER_COMPANY_STATS = 118,
        /// <summary>
        /// The server received a chat message and relays it.
        /// </summary>
        ADMIN_PACKET_SERVER_CHAT = 119,
        /// <summary>
        /// The server's reply to a remove console command.
        /// </summary>
        ADMIN_PACKET_SERVER_RCON = 120,
        /// <summary>
        /// The server gives the admin the data that got printed to its console.
        /// </summary>
        ADMIN_PACKET_SERVER_CONSOLE = 121,
        /// <summary>
        /// The server gives the admin names of all DoCommands.
        /// </summary>
        ADMIN_PACKET_SERVER_CMD_NAMES = 122,
        /// <summary>
        /// The server gives the admin DoCommand information (for logging purposes only)
        /// </summary>
        ADMIN_PACKET_SERVER_CMD_LOGGING = 123,
        /// <summary>
        /// The server gives the admin information from the GameScript in JSON.
        /// </summary>
        ADMIN_PACKET_SERVER_GAMESCRIPT = 124,
        /// <summary>
        /// The server indicates that the remote console command has completed.
        /// </summary>
        ADMIN_PACKET_SERVER_RCON_END = 125,
        /// <summary>
        /// The server replies to a ping request from the admin.
        /// </summary>
        ADMIN_PACKET_SERVER_PONG = 126,
        /// <summary>
        /// For internal reference only, mark the end.
        /// </summary>
        ADMIN_PACKET_SERVER_END = 127,
        /// <summary>
        /// An invalid marker for admin packets.
        /// </summary>
        INVALID_ADMIN_PACKET = 0xFF
    }
}
