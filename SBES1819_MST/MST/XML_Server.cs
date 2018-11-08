using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MST
{
    public class XML_Server
    {
        private ServiceHost _host;

        public XML_Server()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            //TODO: CONFIG
            string address = "net.tcp://localhost:9003/XML_Service";
            _host = new ServiceHost(typeof(XML_Provider));
            _host.AddServiceEndpoint(typeof(IXMLConfiguration_Service), binding, address);
        }

        public void Open()
        {
            try
            {
                _host.Open();
                Console.WriteLine($"XML_Service is started.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'host.Open()'. Error message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void Close()
        {
            try
            {
                _host.Close();
                Console.WriteLine($"XML_Service is stopped.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'host.Close()'. Error message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }
    }
}
