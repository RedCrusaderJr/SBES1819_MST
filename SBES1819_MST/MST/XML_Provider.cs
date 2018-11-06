using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Contracts;

namespace MST
{
    public class XML_Provider : IXMLConfiguration_Service
    {
        // TODO: za sve metode koristi Read() i Write() iz XML_Worker-a
        // one rade sa upisom i citanjem liste<XLM_node> u/iz fajla
        // Jovan i Dunja po 4 komada

        public void AllowProcess(string processName)
        {
            throw new NotImplementedException();
        }

        public void BanGroup(string groupID, string processName)
        {
            throw new NotImplementedException();
        }

        public void BanUser(string userID, string processName)
        {
            throw new NotImplementedException();
        }

        public void BanUserInGroup(string userID, string groupID, string processName)
        {
            throw new NotImplementedException();
        }

        public void ForbidProcess(string processName)
        {
            throw new NotImplementedException();
        }

        public void LiftGroupBan(string groupID, string processName)
        {
            throw new NotImplementedException();
        }

        public void LiftUserBan(string userID, string processName)
        {
            throw new NotImplementedException();
        }

        public void LiftUserInGroupBan(string userID, string groupID, string processName)
        {
            throw new NotImplementedException();
        }
    }
}
