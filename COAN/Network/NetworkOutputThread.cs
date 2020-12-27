using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using NLog;

namespace COAN
{
    public class NetworkOutputThread
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        protected static ConcurrentDictionary<Socket, BlockingCollection<Packet>> queues;

        static NetworkOutputThread()
        {
           queues = new ConcurrentDictionary<Socket, BlockingCollection<Packet>>();
            Thread t = new Thread(Run)
            {
                IsBackground = true
            };
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
        public static void Append(Packet p)
        {
            getQueue(p.getSocket()).Add(p);
        }

        public static void Run()
        {
            while (true)
            {
                foreach (BlockingCollection<Packet> q in queues.Values)
                {
                    try
                    {
                        Packet p = q.Take();

                        /* if the socket is closed, remove it from the queue and leave the foreach */
                        if (!p.getSocket().Connected)
                        {
                            logger.Log(LogLevel.Trace, string.Format("Connection closed - Sending Packet {0}", p.getType()));
                            queues.TryRemove(p.getSocket(), out _);
                            break;
                        }
                        p.Send();
                        logger.Log(LogLevel.Trace, string.Format("Sending Packet {0}", p.getType()));
                    }
                    catch (Exception ex)
                    {
                        logger.Log(LogLevel.Error, string.Format("Sending Packet - Exception: {0}", ex.Message));
                    }
                }
            }
        }
    }
}
