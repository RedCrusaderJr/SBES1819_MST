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
using Manager;
using System.Security.Principal;

namespace MST
{
    public class ThreadFunction
    {
        // ova metoda za string user (iz procesa) proverava da li pripada grupi (string iz xml-a)

        bool IsUserInGroup(string user, string group)
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

            if (user_principal != null)
            {
                // check if user is member of that group
                if (user_principal.IsMemberOf(group_principal))
                {
                    return true;
                }
            }

            return false;
        }

        static string GetProcessOwner(int processId)
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


        public void DetectProcesses()
        {
            while (true)
            {
                Process[] processlist = Process.GetProcesses(Environment.MachineName);

                foreach (Process theprocess in processlist)
                {
                    Console.WriteLine("Process: {0}, process user: {1}", theprocess.ProcessName, GetProcessOwner(theprocess.Id));


                    
                    // obracanje IPS-u zbog detekcije malware-a

                    List<XML_Node> black_list = new List<XML_Node>();       // xml se nalazi u debag folderu
                    black_list = XML_Worker.Instance().XML_Read();          // Poziv iscitavanja

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

        private static void MalwareDetection(Process theprocess)
        {
            //TODO: CONFIG
            string subjectName = "IPSCert";

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
            //EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost/IPS_Service"),
            //                                              new X509CertificateEndpointIdentity(srvCert));

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://10.1.212.157:9001/IPS_Service"),
                                                          new X509CertificateEndpointIdentity(srvCert));

            using (IPS_Client client = new IPS_Client(binding, address))
            {
                // konekcija ka IPS-u
                client.MalwareDetection(GetProcessOwner(theprocess.Id), theprocess.Id.ToString(), theprocess.ProcessName, DateTime.Now);
            }
        }
    }
}
