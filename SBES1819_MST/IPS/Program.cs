using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    class Program
    {
        static void Main(string[] args)
        {
            

            IPS_Server server = new IPS_Server();

            server.Open();
            Console.WriteLine("Press any key to close host...");
            Console.ReadKey();

            server.Close();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
