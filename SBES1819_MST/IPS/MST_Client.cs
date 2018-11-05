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
    public class MST_Client : ChannelFactory<IMST_Service>, IMST_Service
    {
        private IMST_Service proxy;

        public MST_Client(NetTcpBinding binding, EndpointAddress address) 
            : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            proxy = this.CreateChannel();
        }

        public void ProcessShutdown(string userID, string processID)
        {
            try
            {
                proxy.ProcessShutdown(userID, processID);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'ProcessShutdown()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }
    }
}
