using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class MST_Client : ChannelFactory<IMST_Service>, IMST_Service
    {
        public void ProcessShutdown()
        {
            throw new NotImplementedException();
        }
    }
}
