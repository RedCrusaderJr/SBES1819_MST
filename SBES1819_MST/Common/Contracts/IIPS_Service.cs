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
<<<<<<< HEAD
        [OperationContract]
        void MalwareDetection(string userID, string processID, DateTime timeOfDetection);
=======
        [OperationContract(IsOneWay = true)]
        void MalwareDetection(string userID, string processName, DateTime timeOfDetection);
>>>>>>> 9abad58b6bc01611421cb657a375c732fa3f6c1d
    }

    public interface IIPS_ServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProcessShutdownCallback(string userID, string processID);
    }
}
