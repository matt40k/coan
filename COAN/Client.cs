using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAN
{
    public class Client
    {
        public String name;
        public int companyId;
        //public NetworkLanguage language;
        public String address;
        public GameDate joindate;
        public long clientId;


        public Client(long clientId)
        {
            this.clientId = clientId;
        }
    }
}
