using Common;
using Common.Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_Server 
    {
        private ServiceHost _host;
        public static Dictionary<string, ECriticalLevel> MalwareEvents { get; set; } = new Dictionary<string, ECriticalLevel>();

        public IPS_Server()
        {
            
            //string subjectName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string subjectName = "IPSCert";

            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            //string address = "net.tcp://localhost:9001/ISP_Service";
            string address = "net.tcp://10.1.212.157:9001/ISP_Service";

            _host = new ServiceHost(typeof(IPS_Provider));
            _host.AddServiceEndpoint(typeof(IIPS_Service), binding, address);

            _host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;

            _host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            _host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, subjectName);
        }

        public void Open()
        {
            try
            {
                _host.Open();
                Console.WriteLine($"ISP_Service is started.");
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
                Console.WriteLine($"ISP_Service is stopped.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'host.Close()'. Error message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }
    }
}
