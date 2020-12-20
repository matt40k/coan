namespace COAN
{
    public class Client
    {
        public string name;
        public int companyId;
        //public NetworkLanguage language;
        public string address;
        public GameDate joindate;
        public long clientId;

        public Client(long clientId)
        {
            this.clientId = clientId;
        }
    }
}
