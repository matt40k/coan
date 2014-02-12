using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAN
{
    public static class ExtensionMethods
    {
        public static string getDispatchName(this enums.PacketType packet)
        {
            StringBuilder result = new StringBuilder();

            String name = packet.ToString().Replace("ADMIN_PACKET_", "").ToLower();
            //System.out.println("Testing1: "+this.name());
            //System.out.println("Testing2: "+name);
            /* receive packets start at 100 */
            
            if ((int)packet < 100) {
                result = new StringBuilder("send");
            } else {
                result = new StringBuilder("receive");
            }

            foreach (String part in name.Split('_'))
            {
                result.Append(part.Substring(0, 1).ToUpper());
                result.Append(part.Substring(1));
            }

        return result.ToString();
        }
    }
}
