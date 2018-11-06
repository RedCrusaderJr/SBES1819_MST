using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Threading;

namespace MST
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //TODO open host MST

            //TODO open host XML...

            //**************************************************************

            List<XML_Node> lista = new List<XML_Node>();

            lista.Add(new XML_Node("aaa", "aaa", "aaa"));
            lista.Add(new XML_Node("bbb", "bbb", "bbb"));
            lista.Add(new XML_Node("ccc", "ccc", "ccc"));

            XML_Worker.Instance().XML_Write(lista);             // Poziv upisa
                
            List<XML_Node> lista2 = new List<XML_Node>();       // xml se nalazi u debag folderu
            lista2 = XML_Worker.Instance().XML_Read();          // Poziv iscitavanja

            foreach (XML_Node n in lista2)
            {
                Console.WriteLine(n.UserId + " " + n.UserGroup + " " + n.ProcessName);
            }

            //**************************************************************

            ThreadFunction tf = new ThreadFunction();

            Thread t = new Thread(tf.DetectProcesses);
            t.Start();

            Console.ReadLine();
        }
    }
}
