using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace XML_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string mstHostIpAddress = ConfigurationManager.AppSettings["mstIp"];

            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };

            EndpointAddress address = new EndpointAddress($"net.tcp://{mstHostIpAddress}:9003/XML_Service");

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
                    Console.WriteLine("9. View Black list");
                    Console.WriteLine("10. Is Black list valid ? ");
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

                        case 9:
                            List<XML_Node> blackList = client.ViewBlackList();

                            Console.WriteLine("\n***** Current Black List *****\n");
                            foreach(XML_Node node in blackList)
                            {
                                Console.WriteLine($"Process name: {node.ProcessName}, User: {node.UserId}, Group: {node.UserGroup}");
                            }
                            Console.WriteLine("\n******************************\n");

                            break;

                        case 10:
                            if(client.IsBlackListValid())
                            {
                                Console.WriteLine("YAAAAS !");
                            }
                            else
                            {
                                Console.WriteLine("nah. :(");
                            }
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
