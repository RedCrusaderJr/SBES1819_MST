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
            //TODO: ime serverovog sertifikata... .pfx file npr. "IPS_SERVER.pfx"
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9001/ISP_Service";
            _host = new ServiceHost(typeof(IPS_Provider));
            _host.AddServiceEndpoint(typeof(IIPS_Service), binding, address);

            _host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            _host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
            _host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            _host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
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
