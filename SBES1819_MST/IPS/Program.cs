using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
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


        /// <summary>
        /// Manages the remainig entries in dictionary which are out of use.
        /// </summary>
        private static void GarbageCollectorThread()
        {
            if(!Int32.TryParse(ConfigurationManager.AppSettings["garbageThreadSleepPeriod"], out int garbageThreadSleepPeriod))
            {
                garbageThreadSleepPeriod = 30000;
            }

            if (!Int32.TryParse(ConfigurationManager.AppSettings["garbageDeletePeriod"], out int garbageDeletePeriod))
            {
                garbageDeletePeriod = 300000;
            }

            while (true)
            {
                List<string> elementsToRemove = new List<string>();

                foreach (KeyValuePair<string, Pair<ECriticalLevel, DateTime>> p in IPS_Server.MalwareEvents)
                {
                    TimeSpan difference = DateTime.Now - p.Value.Second;

                    if (difference.TotalSeconds > garbageDeletePeriod)
                    {
                        elementsToRemove.Add(p.Key);
                    }
                }

                elementsToRemove.ForEach(e => IPS_Server.MalwareEvents.Remove(e));

                Thread.Sleep(garbageThreadSleepPeriod);
            }
        }
    }
}
