using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract(CallbackContract=typeof(IIPS_ServiceCallback))]
    public interface IIPS_Service
    {
        [OperationContract(IsOneWay = true)]
        void MalwareDetection(string userID, string processName, DateTime timeOfDetection);
    }

    public interface IIPS_ServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProcessShutdownCallback(string userID, string processID);
    }
}
