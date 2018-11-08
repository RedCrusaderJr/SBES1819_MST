using Common;
using Common.Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void MalwareDetection(string userID, string processID, string processName, DateTime timeOfDetection)
        {
            string eventKey = $"{userID}|{processID}";

            if (!IPS_Server.MalwareEvents.ContainsKey(eventKey))
            {
                IPS_Server.MalwareEvents.Add(eventKey, ECriticalLevel.INFORMATION);
                Console.WriteLine("Malware: " + processID + ", process name: " + processName + ", user: " + userID + " ... level: " + IPS_Server.MalwareEvents[eventKey].ToString());

                //LOG
                LogEvent(userID, processID, processName, EventLogEntryType.Information);
            }
            else
            {
                IPS_Server.MalwareEvents[eventKey]++;
                Console.WriteLine("Malware: " + processID + ", process name: " + processName + ", user: " + userID + " ... level: " + IPS_Server.MalwareEvents[eventKey].ToString());

                if (IPS_Server.MalwareEvents[eventKey] == ECriticalLevel.WARNING)
                {
                    //LOG
                    LogEvent(userID, processID, processName, EventLogEntryType.Warning);
                }


                if (IPS_Server.MalwareEvents[eventKey] == ECriticalLevel.CRITICAL)
                {
                    // Windows Lof Manager
                    //LOG
                    LogEvent(userID, processID, processName, EventLogEntryType.Error);

                    // konekcija ka MST-u
                    // gasenje procesa
                    //string subjectName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                    //TODO: CONFIG
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

                    //TODO: CONFIG
                    //EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9002/MST_Service"),
                    //                                              new X509CertificateEndpointIdentity(srvCert));

                    EndpointAddress address = new EndpointAddress(new Uri("net.tcp://10.1.212.159:9002/MST_Service"),
                                                                  new X509CertificateEndpointIdentity(srvCert));

                    using (MST_Client client = new MST_Client(binding, address))
                    {
                        client.ProcessShutdown(userID, processID); //DIMITRIJE: koliko ja vidim ove userID i procesID koje dobijemo te i vracamo
                    }
                }
            }
            
        }

        private void LogEvent(string userID, string processID, string processName, EventLogEntryType type)
        {
            if (!EventLog.SourceExists("CriticalProcesses"))
            {
                EventLog.CreateEventSource("CriticalProcesses", "CriticalProcesses");
            }

            using (EventLog eventLog = new EventLog("CriticalProcesses", Environment.MachineName, "CriticalProcesses"))
            {
                eventLog.WriteEntry("Malware: " + processID + ", process name: " + processName + ", user: " + userID, type, eventLog.Entries.Count + 1);
            }
        }
    }
}
