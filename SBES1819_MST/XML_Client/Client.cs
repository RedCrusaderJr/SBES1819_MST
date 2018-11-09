using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XML_Client
{
    public class Client : ChannelFactory<IXMLConfiguration_Service>, IXMLConfiguration_Service
    {
        private IXMLConfiguration_Service _proxy;

        public Client(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            _proxy = this.CreateChannel();
        }

        public void AllowProcess(string processName)
        {
            try
            {
                _proxy.AllowProcess(processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'AllowProcess()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void BanGroup(string groupID, string processName)
        {
            try
            {
                _proxy.BanGroup(groupID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'BanGroup()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void BanUser(string userID, string processName)
        {
            try
            {
                _proxy.BanUser(userID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'BanUser()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void BanUserInGroup(string userID, string groupID, string processName)
        {
            try
            {
                _proxy.BanUserInGroup(userID, groupID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'BanUserInGroup()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void ForbidProcess(string processName)
        {
            try
            {
                _proxy.ForbidProcess(processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'ForbidProcess()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void LiftGroupBan(string groupID, string processName)
        {
            try
            {
                _proxy.LiftGroupBan(groupID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'LiftGroupBan()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void LiftUserBan(string userID, string processName)
        {
            try
            {
                _proxy.LiftUserBan(userID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'LiftUserBan()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void LiftUserInGroupBan(string userID, string groupID, string processName)
        {
            try
            {
                _proxy.LiftUserInGroupBan(userID, groupID, processName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'LiftUserInGroupBan()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public List<XML_Node> ViewBlackList()
        {
            try
            {
                return _proxy.ViewBlackList();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error on 'ViewBlackList()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
                return null;
            }
        }
    }
}
