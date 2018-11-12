using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Threading;
using Common;

namespace MST
{
    class Program
    {
        
        public static void Main(string[] args)
        {
            Console.ReadLine();
            MST_Server server_MST = new MST_Server();
            server_MST.Open();

            XML_Server server_XML = new XML_Server();
            server_XML.Open();


            InitializeBlacklist();


            ThreadFunction tf = new ThreadFunction();

            Thread t = new Thread(tf.ProcessesMonitor);
            t.Start();


            Console.WriteLine("Press any key to close all hosts...");
            Console.ReadKey();

            server_MST.Close();
            server_XML.Close();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Initialize the list of malware processes.
        /// </summary>
        private static void InitializeBlacklist()
        {
            List<XML_Node> lista = new List<XML_Node>
            {
                new XML_Node("user1", "*", "notepad"),
                new XML_Node("user1", "BanGroup", "GitHubDesktop"),
                new XML_Node("*", "BanGroup", "notepad++"),
                new XML_Node("*", "*", "Taskmgr")
            };

            XML_Worker.Instance().XML_Write(lista);
        }
    }
}
