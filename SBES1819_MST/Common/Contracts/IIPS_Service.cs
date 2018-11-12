using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{ 
    [ServiceContract]
    public interface IIPS_Service
    {
        /// <summary>
        /// Evidenting the malware event by parameters.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        /// <param name="processName"></param>
        /// <param name="timeOfDetection"></param>
        [OperationContract]
        void MalwareDetection(string userID, string processID, string processName, DateTime timeOfDetection);
    }
}
