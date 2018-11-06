using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_CallbackProvider : IIPS_ServiceCallback
    {
        public void ProcessShutdownCallback(string userID, string processID)
        {
            string eventKey = $"{userID}|{processID}";
            IPS_Server.MalwareEvents.Remove(eventKey);
        }
    }
}
