using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;

namespace MST
{
    public class XML_Worker
    {
        static String xml_name = "Blacklist.xml";
        private static XML_Worker _instance;

        private XML_Worker()
        {
            if (!File.Exists(xml_name))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(xml_name))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Nodes");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
            else
            {
                File.Delete(xml_name);
                using (XmlWriter xmlWriter = XmlWriter.Create(xml_name))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Nodes");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
        }

        public static XML_Worker Instance()
        {
            if (_instance == null || !File.Exists(xml_name))
            {
                _instance = new XML_Worker();
            }

            return _instance;
        }

        

        public void XML_Write(List<XML_Node> nodes)
        {
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
        }

        public List<XML_Node> XML_Read()
        {
            List<XML_Node> nodes = new List<XML_Node>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xml_name);

            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
            {

                try
                {
                    nodes.Add(new XML_Node(xmlNode.ChildNodes[0].InnerText, xmlNode.ChildNodes[1].InnerText, xmlNode.ChildNodes[2].InnerText));
                }
                catch
                {
                    return null;
                }
            }

            return nodes;
        }
    }
}
