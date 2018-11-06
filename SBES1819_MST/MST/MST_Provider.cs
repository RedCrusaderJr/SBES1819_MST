using Common.Contracts;
using System;
using System.Collections.Generic;
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
            _callback = OperationContext.Current.GetCallbackChannel<IIPS_ServiceCallback>();
        }

        public void ProcessShutdown(string userID, string processID)
        {
            //TODO: logika za gasenje procesa pomocu processID-a

            _callback.ProcessShutdownCallback(userID, processID);
        }
    }
}
