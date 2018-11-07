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

namespace MST
{
    public class ThreadFunction
    {
        // ova metoda za string user (iz procesa) proverava da li pripada grupi (string iz xml-a)

        bool IsUserInGroup(string user, string group)
        {
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "DOMAINNAME");

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

                NetTcpBinding binding = new NetTcpBinding();
                EndpointAddress address = new EndpointAddress("net.tcp://localhost:9001/ISP_Service");  // TODO: nece biti local host


                foreach (Process theprocess in processlist)
                {
                    Console.WriteLine("Process: {0}, process user: {1}", theprocess.ProcessName, GetProcessOwner(theprocess.Id));

                    // TODO: sastavljanje paketa IPS-u za nedozvoljenu kombinaciju 'processName - user'

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

                                using (IPS_Client client = new IPS_Client(binding, address))
                                {
                                    // konekcija ka IPS-u

                                    client.MalwareDetection(GetProcessOwner(theprocess.Id), theprocess.Id.ToString(), DateTime.Now);
                                }
                            }
                            else if((GetProcessOwner(theprocess.Id) == (Environment.MachineName + "\\" +  n.UserId)) || IsUserInGroup(GetProcessOwner(theprocess.Id), n.UserGroup) == true)
                            {
                                // CASE: user1, *
                                // CASE: * , group1

                                using (IPS_Client client = new IPS_Client(binding, address))
                                {
                                    // konekcija ka IPS-u

                                    client.MalwareDetection(GetProcessOwner(theprocess.Id), theprocess.Id.ToString(), DateTime.Now);
                                }
                            }
                            else
                            {
                                // CASE: * , *

                                using (IPS_Client client = new IPS_Client(binding, address))
                                {
                                    // konekcija ka IPS-u

                                    client.MalwareDetection(GetProcessOwner(theprocess.Id), theprocess.Id.ToString(), DateTime.Now);
                                }
                            }
                        }
                    }

                    
                }

                Thread.Sleep(10000);
            }
        }
    }
}
