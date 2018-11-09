using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string directory = ConfigurationManager.AppSettings["directory"];
            string fileName = $@"{directory}\{ConfigurationManager.AppSettings["filename"]}";
            string userName = ConfigurationManager.AppSettings["username"];

            if (fileName.Contains("IPS"))
            {
                LaunchIPS(fileName, userName);
            }
            else if (fileName.Contains("MST"))
            {
                LaunchMST(fileName, userName);
            }
        }

        public static void LaunchIPS(string fileName, string userName)
        {
            if (!EventLog.SourceExists("CriticalProcesses"))
            {
                EventLog.CreateEventSource("CriticalProcesses", "CriticalProcesses");
            }

            StartApp(fileName, userName);
        }

        public static void LaunchMST(string fileName, string userName)
        {
            StartApp(fileName, userName);
        }

        private static void StartApp(string fileName, string userName)
        {
            Process process = new Process();

            process.StartInfo.UseShellExecute = false;
            //process.StartInfo.Arguments = "args...";
            //process.StartInfo.Domain = "domainname";
            process.StartInfo.FileName = fileName;
            process.StartInfo.UserName = userName;
            SecureString ssPassword = GetUserPassword(userName);
            process.StartInfo.Password = ssPassword;
            process.Start();
        }

        private static SecureString GetUserPassword(string userName)
        {
            Console.WriteLine($"Username: {userName}");
            Console.Write($"Password: ");

            string password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);


            SecureString ssPassword = new SecureString();
            //string password = "munja";
            for (int x = 0; x < password.Length; x++)
            {
                ssPassword.AppendChar(password[x]);
            }
            password = "";

            return ssPassword;
        }
    }
}
