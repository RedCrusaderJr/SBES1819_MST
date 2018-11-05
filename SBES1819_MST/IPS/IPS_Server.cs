using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_Server 
    {
        public static Dictionary<string, ECriticalLevel> MalwareEvents { get; set; } = new Dictionary<string, ECriticalLevel>();

        public IPS_Server()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/ISP_Service";
            ServiceHost host = new ServiceHost(typeof(IPS_Provider));
            host.AddServiceEndpoint(typeof(IIPS_Service), binding, address);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            //host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertivficationValidation();
        }

        public void Open()
        {

        }

        public void Close()
        {

        }
    }
}
