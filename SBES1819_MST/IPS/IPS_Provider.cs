using Common;
using Common.Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class IPS_Provider : IIPS_Service
    {
        public void MalwareDetection(string userID, string processID, DateTime timeOfDetection)
        {
            string eventKey = $"{userID}|{processID}";

            if (!IPS_Server.MalwareEvents.ContainsKey(eventKey))
            {
                IPS_Server.MalwareEvents.Add(eventKey, ECriticalLevel.INFORMATION);
            }
            else
            {
                IPS_Server.MalwareEvents[eventKey]++;

                if(IPS_Server.MalwareEvents[eventKey] == ECriticalLevel.CRITICAL)
                {
                    // konekcija ka MST-u
                    // gasenje procesa
                    //string subjectName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                    string subjectName = "MSTCert";

                    NetTcpBinding binding = new NetTcpBinding()
                    {
                        CloseTimeout = new TimeSpan(0, 60, 0),
                        OpenTimeout = new TimeSpan(0, 60, 0),
                        ReceiveTimeout = new TimeSpan(0, 60, 0),
                        SendTimeout = new TimeSpan(0, 60, 0),
                    };
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                    X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, subjectName);
                   

                    EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9002/MST_Service"),
                                                                  new X509CertificateEndpointIdentity(srvCert)); //TODO: nece biti localhost

                    using (MST_Client client = new MST_Client(binding, address))
                    {
                        client.ProcessShutdown(userID, processID); //DIMITRIJE: koliko ja vidim ove userID i procesID koje dobijemo te i vracamo
                    }

                    //TODO event log
                }
            }
            
        }
    }
}
