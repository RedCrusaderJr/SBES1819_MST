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
            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:9003/XML_Service");  //TODO: nece biti localhost

            int input;

            using (Client client = new Client(binding, address))
            {
                do
                {
                    Console.WriteLine("1. Allow Process");
                    Console.WriteLine("2. Ban Group");
                    Console.WriteLine("3. Ban User");
                    Console.WriteLine("4. Ban User In Group");
                    Console.WriteLine("5. Forbid Process");
                    Console.WriteLine("6. Lift Group Ban");
                    Console.WriteLine("7. Lift User Ban");
                    Console.WriteLine("8. Lift User In Group Ban");
                    Console.WriteLine("0. Exit");

                    if (!Int32.TryParse(Console.ReadLine(), out input))
                    {
                        input = 9;
                    }

                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("Enter process name: ");
                            client.AllowProcess(Console.ReadLine());
                            break;
                        case 2:
                            Console.WriteLine("Enter process name: ");
                            string process = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            string group = Console.ReadLine();
                            client.BanGroup(group, process);
                            break;
                        case 3:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            Console.WriteLine("Enter user name: ");
                            string user = Console.ReadLine();
                            client.BanUser(user, process);
                            break;
                        case 4:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine();
                            client.BanUserInGroup(user, group, process);
                            break;
                        case 5:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            client.ForbidProcess(process);
                            break;
                        case 6:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine(); 
                            client.LiftGroupBan(group, process);
                            break;
                        case 7:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            client.LiftUserBan(user, process);
                            break;
                        case 8:
                            Console.WriteLine("Enter process name: ");
                            process = Console.ReadLine();
                            Console.WriteLine("Enter user name: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Enter group name: ");
                            group = Console.ReadLine();
                            client.LiftUserInGroupBan(user, group, process);
                            break;
                        case 0:
                            break;
                        default:
                            Console.WriteLine("Wrong input!");
                            break;
                    }
                }
                while (input != 0);
            }
        }
    }
}
