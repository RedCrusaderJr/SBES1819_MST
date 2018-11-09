using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Contracts;

namespace MST
{
    public class XML_Provider : IXMLConfiguration_Service
    {
        // metode pomocu kojih XML_Client manipulise BlackList-om
        // za sve metode koristi Read() i Write() iz XML_Worker-a
        // one rade sa upisom i citanjem liste<XLM_node> u/iz fajla


        private List<XML_Node> black_list = null;   // TODO: Dunja - da li ovo treba u konstruktoru klase ? 

        public XML_Provider()
        {
            if((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }
        }

        public void AllowProcess(string processName)
        {
            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            // black_list.RemoveAll(n => (n.ProcessName == processName));       // brisu se svi node-ovi koji sadrze zabranu za taj proces

            black_list.RemoveAll(n => ((n.UserId == "*") && (n.UserGroup == "*") && (n.ProcessName == processName)));

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void BanGroup(string groupID, string processName)
        {
            bool exist = false;

            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            XML_Node n = new XML_Node("*", groupID, processName);
            
            foreach(XML_Node element in black_list)
            {
                if((element.UserId == n.UserId) && (element.UserGroup == n.UserGroup) && (element.ProcessName == n.ProcessName))
                {
                    exist = true;
                    break;
                }
            }

            if(!exist)
            {
                black_list.Add(n);
            }

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void BanUser(string userID, string processName)
        {
            bool exist = false;

            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            XML_Node n = new XML_Node(userID, "*", processName);

            foreach (XML_Node element in black_list)
            {
                if ((element.UserId == n.UserId) && (element.UserGroup == n.UserGroup) && (element.ProcessName == n.ProcessName))
                {
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                black_list.Add(n);
            }

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void BanUserInGroup(string userID, string groupID, string processName)
        {
            bool exist = false;

            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            XML_Node n = new XML_Node(userID, groupID, processName);

            foreach (XML_Node element in black_list)
            {
                if ((element.UserId == n.UserId) && (element.UserGroup == n.UserGroup) && (element.ProcessName == n.ProcessName))
                {
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                black_list.Add(n);
            }

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void ForbidProcess(string processName)
        {
            bool exist = false;

            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            XML_Node n = new XML_Node("*", "*", processName);

            foreach (XML_Node element in black_list)
            {
                if ((element.UserId == n.UserId) && (element.UserGroup == n.UserGroup) && (element.ProcessName == n.ProcessName))
                {
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                black_list.Add(n);
            }

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void LiftGroupBan(string groupID, string processName)
        {
            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            black_list.RemoveAll(n => ((n.UserId == "*") && (n.UserGroup == groupID) && (n.ProcessName == processName)));

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void LiftUserBan(string userID, string processName)
        {
            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            black_list.RemoveAll(n => ((n.UserId == userID) && (n.UserGroup == "*") && (n.ProcessName == processName)));

            XML_Worker.Instance().XML_Write(black_list);
        }

        public void LiftUserInGroupBan(string userID, string groupID, string processName)
        {
            if ((black_list = XML_Worker.Instance().XML_Read()) == null)
            {
                Console.WriteLine("Error while reading Black List from file.");
                black_list = new List<XML_Node>();
            }

            black_list.RemoveAll(n => ((n.UserId == userID) && (n.UserGroup == groupID) && (n.ProcessName == processName)));

            XML_Worker.Instance().XML_Write(black_list);
        }

        public List<XML_Node> ViewBlackList()
        {
            return black_list;
        }
    }
}
