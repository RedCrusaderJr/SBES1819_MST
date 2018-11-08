using System;
using System.Collections.Generic;
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
            if (!EventLog.SourceExists("CriticalProcesses"))
            {
                EventLog.CreateEventSource("CriticalProcesses", "CriticalProcesses");
            }

            Process process = new Process();
            
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = @"App\IPS.exe";
            //process.StartInfo.Arguments = "args...";
            //process.StartInfo.Domain = "domainname";
            process.StartInfo.UserName = "IPSServer";

            SecureString ssPwd = new SecureString();
            string password = "munja";
            for (int x = 0; x < password.Length; x++)
            {
                ssPwd.AppendChar(password[x]);
            }
            password = "";
            process.StartInfo.Password = ssPwd;
            process.Start();
        }
    }
}
