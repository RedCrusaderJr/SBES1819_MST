using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace Common.Contracts
{
    [ServiceContract]
    public interface IXMLConfiguration_Service
    {
        [OperationContract]
        void BanUser(string userID, string processName);

        [OperationContract]
        void LiftUserBan(string userID, string processName);



        [OperationContract]
        void BanGroup(string groupID, string processName);

        [OperationContract]
        void LiftGroupBan(string groupID, string processName);


        [OperationContract]
        void BanUserInGroup(string userID, string groupID, string processName);

        [OperationContract]
        void LiftUserInGroupBan(string userID, string groupID, string processName);


        [OperationContract]
        void ForbidProcess(string processName);

        [OperationContract]
        void AllowProcess(string processName);


        [OperationContract]
        List<XML_Node> ViewBlackList();

        [OperationContract]
        bool IsBlackListValid();
    }
}
