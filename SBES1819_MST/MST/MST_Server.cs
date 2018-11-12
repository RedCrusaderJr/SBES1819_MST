using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Manager;

namespace MST
{
    public class MST_Server
    {
        private ServiceHost _host;

        /// <summary>
        /// Initialize MST host for WCF communication with certificates.
        /// </summary>
        public MST_Server()
        {
            string mstHostIpAddress = ConfigurationManager.AppSettings["mstIp"];
            string mstCertName = ConfigurationManager.AppSettings["mstCertName"];
            
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = $"net.tcp://{mstHostIpAddress}/MST_Service";
            
            _host = new ServiceHost(typeof(MST_Provider));
            _host.AddServiceEndpoint(typeof(IMST_Service), binding, address);

            _host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;

            _host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            _host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, mstCertName);
        }

        /// <summary>
        /// Opens the MSt host.
        /// </summary>
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

        /// <summary>
        /// Closes the MST host.
        /// </summary>
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
