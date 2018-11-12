using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MST
{
    public class MST_Provider : IMST_Service
    {
        /// <summary>
        /// Shutdowns the process with processID.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        public void ProcessShutdown(string userID, string processID)
        {
            Process[] processlist = Process.GetProcesses(Environment.MachineName);

            foreach (Process p in processlist)
            {
                if (p.Id == Int32.Parse(processID))
                {
                    p.Kill();

                    // break;
                    // da bi gasio vise procesa pokrenutih od strane 1 programa
                }
            }
        }
    }
}
