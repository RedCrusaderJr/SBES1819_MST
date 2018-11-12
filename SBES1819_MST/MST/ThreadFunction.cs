using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Common.Manager;
using System.Security.Principal;
using System.Configuration;
using Common;

namespace MST
{
    public class ThreadFunction
    {
        /// <summary>
        /// Repeatedly checks if there is an active malware processe(s).
        /// </summary>
        public void ProcessesMonitor()
        {
            while (true)
            {
                Process[] processlist = Process.GetProcesses(Environment.MachineName);

                foreach (Process theprocess in processlist)
                {
                    Console.WriteLine("Process: {0}, process user: {1}", theprocess.ProcessName, GetProcessOwner(theprocess.Id));

                    // obracanje IPS-u zbog detekcije malware-a

                    List<XML_Node> black_list = new List<XML_Node>();
                    black_list = XML_Worker.Instance().XML_Read();

                    foreach (XML_Node n in black_list)
                    {
                        // Console.WriteLine(n.UserId + " " + n.UserGroup + " " + n.ProcessName);

                        if(theprocess.ProcessName == n.ProcessName)
                        {
                            if((GetProcessOwner(theprocess.Id) == (Environment.MachineName + "\\" + n.UserId)) && IsUserInGroup(GetProcessOwner(theprocess.Id), n.UserGroup) == true)
                            {
                                // CASE: user1, group1
                                MalwareDetection(theprocess);
                            }
                            else if((GetProcessOwner(theprocess.Id) == (Environment.MachineName + "\\" +  n.UserId)) || IsUserInGroup(GetProcessOwner(theprocess.Id), n.UserGroup) == true)
                            {
                                // CASE: user1, *
                                // CASE: * , group1
                                MalwareDetection(theprocess);
                            }
                            else if((n.UserId == "*") && (n.UserGroup == "*"))
                            {
                                // CASE: * , *
                                MalwareDetection(theprocess);
                            }
                        }
                    }
                }
                

                Console.WriteLine("\n******************** END OF PASS ********************\n");

                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Connects to IPS with a request to evident a malware process.
        /// </summary>
        /// <param name="process"></param>
        private void MalwareDetection(Process process)
        {
            string ipsCertName = ConfigurationManager.AppSettings["ipsCertName"];
            string ipsHostIpAddress = ConfigurationManager.AppSettings["ipsIp"];

            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 60, 0),
                OpenTimeout = new TimeSpan(0, 60, 0),
                ReceiveTimeout = new TimeSpan(0, 60, 0),
                SendTimeout = new TimeSpan(0, 60, 0),
            };
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, ipsCertName);

            EndpointAddress address = new EndpointAddress(new Uri($"net.tcp://{ipsHostIpAddress}:9001/IPS_Service"),
                                                          new X509CertificateEndpointIdentity(srvCert));
            
            using (IPS_Client client = new IPS_Client(binding, address))
            {
                client.MalwareDetection(GetProcessOwner(process.Id), process.Id.ToString(), process.ProcessName, DateTime.Now);
            }
        }
        
        /// <summary>
        /// Checks whether user belongs to a group.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private bool IsUserInGroup(string user, string group)
        {
            if (group == "*")
            {
                return false;
            }

            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName);

            // find a user
            UserPrincipal user_principal  = UserPrincipal.FindByIdentity(ctx, user);

            // find the group in question
            GroupPrincipal group_principal = GroupPrincipal.FindByIdentity(ctx, group);

            if (user_principal != null && group_principal != null)
            {
                // check if user is member of that group
                if (user_principal.IsMemberOf(group_principal))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the name of the owner of process with ID (processId).
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        private string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
