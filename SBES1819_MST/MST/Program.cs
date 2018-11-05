using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace MST
{
    class Program
    {
        static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }

        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses(Environment.MachineName);

            foreach (Process theprocess in processlist)
            {
                if(theprocess.ProcessName == "notepad")
                Console.WriteLine("Process: {0} ID: {1}, process user: {2}", theprocess.ProcessName, theprocess.Id, GetProcessOwner(theprocess.Id));
            }

            Console.ReadLine();
        }
    }
}
