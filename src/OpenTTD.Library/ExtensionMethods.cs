using System.Text;

namespace org.openttd
{
    public static class ExtensionMethods
    {
        public static string GetDispatchName(this PacketType packet)
        {
            StringBuilder result;
            var name = packet.ToString().Replace("ADMIN_PACKET_", "").ToLower();

            /* receive packets start at 100 */
            if ((int)packet < 100)
                result = new StringBuilder("send");
            else
                result = new StringBuilder("receive");

            foreach (string part in name.Split('_'))
            {
                result.Append(part.Substring(0, 1).ToUpper());
                result.Append(part.Substring(1));
            }

            return result.ToString();
        }
    }
}
