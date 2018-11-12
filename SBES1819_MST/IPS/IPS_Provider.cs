using Common;
using Common.Contracts;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// <summary>
        /// Evidenting the malware event by parameters.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        /// <param name="processName"></param>
        /// <param name="timeOfDetection"></param>
        public void MalwareDetection(string userID, string processID, string processName, DateTime timeOfDetection)
        {
            string eventKey = $"{userID}|{processID}";

            lock (IPS_Server.lockObject)
            {

                if (!IPS_Server.MalwareEvents.ContainsKey(eventKey))
                {
                    IPS_Server.MalwareEvents.Add(eventKey, new Pair<ECriticalLevel, DateTime>(ECriticalLevel.INFORMATION, timeOfDetection));

                    Console.WriteLine("Malware: " + processID + ", process name: " + processName + ", user: " + userID + " ... level: " + IPS_Server.MalwareEvents[eventKey].First.ToString());
                    LogMalwareEvent(userID, processID, processName, EventLogEntryType.Information);
                }
                else if (IPS_Server.MalwareEvents[eventKey].First == ECriticalLevel.INFORMATION)
                {
                    IPS_Server.MalwareEvents[eventKey].First++;

                    Console.WriteLine("Malware: " + processID + ", process name: " + processName + ", user: " + userID + " ... level: " + IPS_Server.MalwareEvents[eventKey].First.ToString());
                    LogMalwareEvent(userID, processID, processName, EventLogEntryType.Warning);
                }
                else
                {
                    if (IPS_Server.MalwareEvents[eventKey].First == ECriticalLevel.WARNING)
                    {
                        IPS_Server.MalwareEvents[eventKey].First++;
                    }
                    
                    Console.WriteLine("Malware: " + processID + ", process name: " + processName + ", user: " + userID + " ... level: " + IPS_Server.MalwareEvents[eventKey].First.ToString());
                    LogMalwareEvent(userID, processID, processName, EventLogEntryType.Error);

                    ShutdownMalwareProcess(userID, processID);
                }
            }
        }

        /// <summary>
        /// Connects to MST with a request to shutdown the process with processID.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        private void ShutdownMalwareProcess(string userID, string processID)
        {
            string mstCertName = ConfigurationManager.AppSettings["mstCertName"];
            string mstHostIpAddress = ConfigurationManager.AppSettings["mstIp"];

            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, mstCertName);
            EndpointAddress address = new EndpointAddress(new Uri($"net.tcp://{mstHostIpAddress}:9002/MST_Service"),
                                                          new X509CertificateEndpointIdentity(srvCert));

            using (MST_Client client = new MST_Client(binding, address))
            {
                client.ProcessShutdown(userID, processID);
            }
        }

        /// <summary>
        /// Encapsulates logging of malware events.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        /// <param name="processName"></param>
        /// <param name="type"></param>
        private void LogMalwareEvent(string userID, string processID, string processName, EventLogEntryType type)
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
