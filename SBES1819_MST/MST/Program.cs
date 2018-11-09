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


            // **********************************************************************************

            List<IXML_Node> lista = new List<IXML_Node>();

            lista.Add(new XML_Node("user1", "*", "notepad"));
            lista.Add(new XML_Node("user1", "BanGroup", "GitHubDesktop"));
            lista.Add(new XML_Node("*", "BanGroup", "notepad++"));
            lista.Add(new XML_Node("*", "*", "Taskmgr"));

            XML_Worker.Instance().XML_Write(lista);             // Poziv upisa
            

            // **********************************************************************************



            ThreadFunction tf = new ThreadFunction();

            Thread t = new Thread(tf.DetectProcesses);
            t.Start();
             


            Console.WriteLine("Press any key to close all hosts...");
            Console.ReadKey();

            // close hosts MST

            server_MST.Close();
            server_XML.Close();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
