using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;
using Manager;

namespace MST
{
    public class MST_Server
    {
        private ServiceHost _host;

        public MST_Server()
        {
            // TODO: komunikacija preko sertifikata




            // string srvCertCN = Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            // binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/MST_Service";
            _host = new ServiceHost(typeof(MST_Provider));
            _host.AddServiceEndpoint(typeof(IMST_Service), binding, address);

            //_host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            //_host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            // _host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            // _host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
        }

        public void Open()
        {
            try
            {
                _host.Open();
                Console.WriteLine($"MST_Service is started.");
            }
            catch(Exception e)
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
                Console.WriteLine($"MST_Service is stopped.");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error on 'host.Close()'. Error message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }
    }
}
