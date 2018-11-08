using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XML_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            //TODO: CONFIG
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:9003/XML_Service");  //TODO: nece biti localhost

            int input;

            using (Client client = new Client(binding, address))
            {
                do
                {
                    Console.WriteLine("1. Ban User");
                    Console.WriteLine("2. Ban Group");
                    Console.WriteLine("3. Ban User In Group");
                    Console.WriteLine("4. Forbid Process");
                    Console.WriteLine();
                    Console.WriteLine("5. Lift User Ban");
                    Console.WriteLine("6. Lift Group Ban");
                    Console.WriteLine("7. Lift User In Group Ban");
                    Console.WriteLine("8. Allow Process");
                    Console.WriteLine();
                    Console.WriteLine("0. Exit");
                    Console.WriteLine();

                    if (!Int32.TryParse(Console.ReadLine(), out input))
                    {
                        input = 9;
                    }

                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("Enter user name: ");
                            string user = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            string process = Console.ReadLine();
                            client.BanUser(user, process);
                            break;

                        case 2:
                            Console.WriteLine("Enter group name: ");
                            string group = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.BanGroup(group, process);
                            break;

                        case 3:
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.BanUserInGroup(user, group, process);
                            break;

                        case 4:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.ForbidProcess(process);
                            break;

                        case 5:
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.LiftUserBan(user, process);
                            break;

                        case 6:
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.LiftGroupBan(group, process);
                            break;

                        case 7:
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine();
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.LiftUserInGroupBan(user, group, process);
                            break;

                        case 8:
                            Console.WriteLine("Enter process name: ");
                            client.AllowProcess(Console.ReadLine());
                            break;

                        case 0:
                            break;

                        default:
                            Console.WriteLine("Wrong input!");
                            break;
                    }

                    Console.WriteLine();
                }
                while (input != 0);
            }
        }
    }
}
