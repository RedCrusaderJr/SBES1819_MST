using Common;
using Common.Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_Server 
    {
        private ServiceHost _host;
        public static Dictionary<string, Pair<ECriticalLevel, DateTime>> MalwareEvents { get; set; } = new Dictionary<string, Pair<ECriticalLevel, DateTime>>();

        public IPS_Server()
        {
            //TODO: CONFIG
            //string subjectName = "IPSCert";

            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            //TODO: CONFIG
            string address = $"net.tcp://{ConfigurationManager.AppSettings["ipsIp"]}:9001/IPS_Service";
            //string address = "net.tcp://10.1.212.157:9001/IPS_Service";

            _host = new ServiceHost(typeof(IPS_Provider));
            _host.AddServiceEndpoint(typeof(IIPS_Service), binding, address);

            _host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            _host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            _host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, ConfigurationManager.AppSettings["ipsCertName"]);

            ServiceSecurityAuditBehavior serviceSecurityAuditBehavior = new ServiceSecurityAuditBehavior();
            _host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            _host.Description.Behaviors.Add(serviceSecurityAuditBehavior);

            _host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            _host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

        }

        public void Open()
        {
            try
            {
                _host.Open();
                Console.WriteLine($"IPS_Service is started.");
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
                Console.WriteLine($"IPS_Service is stopped.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'host.Close()'. Error message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }
    }
}
