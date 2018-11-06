using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    //TODO proxy ka MST


                    //TODO event log

                    //OVO U CALLBACK
                    IPS_Server.MalwareEvents.Remove(eventKey);
                }
            }
            
        }
    }
}
