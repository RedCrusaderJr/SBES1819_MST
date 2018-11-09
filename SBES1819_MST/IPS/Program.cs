using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPS
{
    class Program
    {
        static void Main(string[] args)
        {
            IPS_Server server = new IPS_Server();

            Thread t = new Thread(GarbageCollectorThread);
            t.IsBackground = true;
            t.Start();

            server.Open();
            Console.WriteLine("Press any key to close host...");
            Console.ReadKey();

            server.Close();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }



        private static void GarbageCollectorThread()
        {
            while(true)
            {
                List<string> elementsToRemove = new List<string>();

                foreach (KeyValuePair<string, Pair<ECriticalLevel, DateTime>> p in IPS_Server.MalwareEvents)
                {
                    TimeSpan difference = DateTime.Now - p.Value.Second;

                    if (difference.TotalSeconds > 30)
                    {
                        elementsToRemove.Add(p.Key);
                    }
                }

                elementsToRemove.ForEach(e => IPS_Server.MalwareEvents.Remove(e));

                Thread.Sleep(30000);
            }
        }
    }
}
