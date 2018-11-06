using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_Provider : IIPS_Service
    {
        public void MalwareDetection(string userID, string processID, DateTime timeOfDetection)
        {
            string eventKey = $"{userID}|{processID}";

            if (!IPS_Server.MalwareEvents.ContainsKey(eventKey))
            {
                IPS_Server.MalwareEvents.Add(eventKey, ECriticalLevel.INFORMATION);
            }
            else
            {
                IPS_Server.MalwareEvents[eventKey]++;

                if(IPS_Server.MalwareEvents[eventKey] == ECriticalLevel.CRITICAL)
                {
                    NetTcpBinding binding = new NetTcpBinding();
                    EndpointAddress address = new EndpointAddress("net.tcp://localhost:9002/MST_Service"); //TODO: nece biti localhost
                    using (MST_Client client = new MST_Client(binding, address))
                    {
                        client.ProcessShutdown(userID, processID); //DIMITRIJE: koliko ja vidim ove userID i procesID koje dobijemo te i vracamo
                    }

                    //TODO event log
                }
            }
            
        }
    }
}
