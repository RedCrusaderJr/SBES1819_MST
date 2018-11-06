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

namespace MST
{
    public class IPS_Client : ChannelFactory<IIPS_Service>, IIPS_Service, IDisposable
    {
        private IIPS_Service _proxy;

        public IPS_Client(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
<<<<<<< HEAD
            // TODO: komunikacija preko sertifikata

            //string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            //this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            //this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            //this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
=======
            // TODO: ime klijentskog sertifikata.... .pfx file npr. "IPS_CLIENT.pfx"
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
>>>>>>> 9abad58b6bc01611421cb657a375c732fa3f6c1d

            _proxy = this.CreateChannel();
        }

        public void MalwareDetection(string userID, string processID, DateTime timeOfDetection)
        {
            try
            {
                _proxy.MalwareDetection(userID, processID, timeOfDetection);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error on 'MalwareDetection()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void Dispose()
        {
            if (_proxy != null)
            {
                _proxy = null;
            }

            this.Close();
        }
    }
}
