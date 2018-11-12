using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using Common;

namespace MST
{
    public class XML_Worker
    {
        #region Instance
        private static XML_Worker _instance;

        public static XML_Worker Instance()
        {
            if (_instance == null || !File.Exists(xml_name))
            {
                _instance = new XML_Worker();
            }

            return _instance;
        }

        private XML_Worker()
        {
            if (File.Exists(xml_name))
            {
                File.Delete(xml_name);
            }

            using (XmlWriter xmlWriter = XmlWriter.Create(xml_name))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Nodes");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
        #endregion

        private static string xml_name = ConfigurationManager.AppSettings["xmlName"];

        private int hashCode = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blackList"></param>
        /// <returns></returns>
        private bool CheckHash(List<XML_Node> blackList)
        {
            string hashString = GetHashString(blackList);

            if (hashCode == hashString.GetHashCode())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private string GetHashString(List<XML_Node> nodes)
        {
            string hashString = "";

            foreach (XML_Node n in nodes)
            {
                hashString += n.UserId + n.UserGroup + n.ProcessName;
            }

            return hashString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        public void XML_Write(List<XML_Node> nodes)
        {
            File.Delete(xml_name);
            using (XmlWriter xmlWriter = XmlWriter.Create(xml_name))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Nodes");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xml_name);

            foreach (XML_Node node in nodes)
            {
                XmlNode xml_node = xmlDocument.CreateElement("node");

                xmlDocument.DocumentElement.AppendChild(xml_node);

                XmlNode userId = xmlDocument.CreateElement("userId");
                userId.InnerText = node.UserId;
                xml_node.AppendChild(userId);

                XmlNode userGroup = xmlDocument.CreateElement("userGroup");
                userGroup.InnerText = node.UserGroup;
                xml_node.AppendChild(userGroup);

                XmlNode processName = xmlDocument.CreateElement("processName");
                processName.InnerText = node.ProcessName;
                xml_node.AppendChild(processName);

            }

            xmlDocument.Save(xml_name);

            hashCode = GetHashString(nodes).GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<XML_Node> XML_Read()
        {
            List<XML_Node> nodes = new List<XML_Node>();

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(xml_name);
            }
            catch
            {
                return new List<XML_Node>();
            }

            XmlNodeList xmlNodeList;

            try
            {
                xmlNodeList = xmlDocument.DocumentElement.ChildNodes;
            }
            catch
            {
                return new List<XML_Node>();
            }

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    nodes.Add(new XML_Node(xmlNode.ChildNodes[0].InnerText, xmlNode.ChildNodes[1].InnerText, xmlNode.ChildNodes[2].InnerText));
                }
                catch
                {
                    return new List<XML_Node>();
                }
            }

            if (CheckHash(nodes))
            {
                return nodes;
            }
            else
            {
                Console.WriteLine("Blacklist was changed by someone unreliable!");
                return new List<XML_Node>();
            }
        }

        /// <summary>
        /// metoda ista kao XML_Read(), samo sto umesto List<XML_Node> vraca bool 
        /// treba nam za proveru Black Liste od strane XML_Client - a
        /// </summary>
        /// <returns></returns>
        public bool ValidateBlackList()
        {
            List<XML_Node> nodes = new List<XML_Node>();

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(xml_name);
            }
            catch
            {
                return false;
            }

            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
            {

                try
                {
                    nodes.Add(new XML_Node(xmlNode.ChildNodes[0].InnerText, xmlNode.ChildNodes[1].InnerText, xmlNode.ChildNodes[2].InnerText));
                }
                catch
                {
                    return false;
                }
            }

            if (CheckHash(nodes))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
