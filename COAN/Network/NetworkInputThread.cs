using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using NLog;

namespace COAN
{
    public class NetworkInputThread
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        protected static ConcurrentDictionary<Socket, BlockingCollection<Packet>> queues;

        static NetworkInputThread()
        {
           queues = new ConcurrentDictionary<Socket, BlockingCollection<Packet>>();
           System.Threading.Thread t = new System.Threading.Thread(run);
           t.IsBackground = true;
           t.Start();
           }

        protected static BlockingCollection<Packet> getQueue(Socket socket)
        {
            if (queues.ContainsKey(socket) == false)
            {
                queues.TryAdd(socket, new BlockingCollection<Packet>(100));
            }
            return queues[socket];
        }

        public static Packet getNext(Socket socket)
        {
            return getQueue(socket).Take();
        }

        /**
        * Append a packet to the appropriate queue.
        * @param p Packet to append to the queue.
        */
        public static void append(Packet p)
        {
            getQueue(p.getSocket()).Add(p);
        }

        public static void run()
        {
            while (true)
            {
                foreach (Socket socket in queues.Keys)
                {
                    try
                    {
                        if (socket.Connected == false)
                        {
                            queues.TryRemove(socket, out _);
                            logger.Log(LogLevel.Trace, string.Format("Socket closed: "));
                            continue;
                        }

                        Packet p = new Packet(socket);
                        append(p);
                        logger.Log(LogLevel.Trace, string.Format("Received Packet: {0}", p.getType()));
                    }
                    catch (Exception ex)
                    {
                        logger.Log(LogLevel.Error, string.Format("Failed reading packet", ex));
                    }
                }
            }
        }

    } // END CLASSc
}
