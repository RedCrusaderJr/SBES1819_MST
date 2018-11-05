using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MST
{
    public class IPS_Client : ChannelFactory<IIPS_Service>, IIPS_Service
    {
        public void MalwareDetection(string userID, string processName, DateTime timeOfDetection)
        {
            throw new NotImplementedException();
        }
    }
}
