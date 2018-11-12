using Common.Contracts;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class MST_Client : ChannelFactory<IMST_Service>, IMST_Service, IDisposable
    {
        private IMST_Service _proxy;

        /// <summary>
        /// Initialize the WCF communication channel to MST host.
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="address"></param>
        public MST_Client(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            string ipsCertName = ConfigurationManager.AppSettings["ipsCertName"];
            
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, ipsCertName);

            _proxy = this.CreateChannel();
        }

        /// <summary>
        /// Encapsulates the call of ProcessShutdown() method on MST host.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        public void ProcessShutdown(string userID, string processID)
        {
            try
            {
                _proxy.ProcessShutdown(userID, processID);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'ProcessShutdown()' with message: {e.Message}");
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
