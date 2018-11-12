using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IMST_Service
    {
        /// <summary>
        /// Shutdowns the process with processID.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        [OperationContract]
        void ProcessShutdown(string userID, string processID);
    }
}
