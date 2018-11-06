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
        private IIPS_ServiceCallback _callback = null;

        public MST_Provider()
        {
            // TODO: da li ce 'OperationContext.Current' vaziti na razlicitim racunarima ?

            _callback = OperationContext.Current.GetCallbackChannel<IIPS_ServiceCallback>();
        }

        public void ProcessShutdown(string userID, string processID)
        {
            Process[] processlist = Process.GetProcesses(Environment.MachineName);

            foreach(Process p in processlist)
            {
                if(p.Id == Int32.Parse(processID))
                {
                    p.Kill();
                    break;
                }
            }


            // nakon izvrsene logike, salje se callback IPS-u

            _callback.ProcessShutdownCallback(userID, processID);
        }
    }
}
