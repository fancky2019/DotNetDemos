using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AuthCommon
{
    public class XMLHelper
    {
        public static XMLHelper Instance { get; set; }
        static XMLHelper()
        {
            Instance = new XMLHelper();
        }

        private XMLHelper()
        {

        }

        /// <summary>
        /// 创建xml
        /// </summary>
        /// <param name="path"></param>
        public void CreateXML(string path)
        {

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            //根节点  
            XmlElement root = doc.CreateElement(Path.GetFileNameWithoutExtension(path));
            doc.AppendChild(root);
            doc.Save(path);
        }
  
        /// <summary>
        /// 写入新的数据，覆盖之前数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="publicKey"></param>
        public void WriteDatas(string primaryKey, string publicKey, string path)
        {
            if (!File.Exists(path))
            {
                CreateXML(path);
            }
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element(Path.GetFileNameWithoutExtension(path));
            var nodes = root.Elements();
            if (nodes.Count() > 0)
            {
                nodes.Remove();
            }
            XElement element = new XElement("Data",
                                new XElement("PrimaryKey", primaryKey),
                                new XElement("PublicKey", publicKey));
            root.Add(element);
            doc.Save(path);

            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(path);
            //XmlNode root = xmlDocument.SelectSingleNode("Keys");
            //XmlNode node = xmlDocument.CreateNode("element", "Data", "");
            //int count1 = t.Count();
            ////PrimaryKey
            //XmlElement primaryKeyElement = xmlDocument.CreateElement("PrimaryKey");
            //primaryKeyElement.InnerText = primaryKey;
            //node.AppendChild(primaryKeyElement);
            ////PublicKey
            //XmlElement publicKeyElement = xmlDocument.CreateElement("PublicKey");
            //publicKeyElement.InnerText = publicKey;
            //node.AppendChild(publicKeyElement);
            //root.AppendChild(node);
            //xmlDocument.Save(path);

        }


        /// <summary>
        /// 读取xml,List[0]:PrimaryKey,List[1]:PublicKey
        /// </summary>
        /// <returns></returns>
        public List<string> ReadXMLData(string path)
        {
            List<string> priPubKeys = new List<string>();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Keys.xml");
            if (!File.Exists(Path.Combine(path)))
            {
                return priPubKeys;
            }
            XDocument doc = XDocument.Load(path);
            IEnumerable<XElement> node = doc.Element(Path.GetFileNameWithoutExtension(path)).Elements("Data");
            foreach (XElement element in node)
            {
                string primaryKey = element.Elements("PrimaryKey").FirstOrDefault().Value;
                string publicKey = element.Elements("PublicKey").FirstOrDefault().Value;
                priPubKeys.Add(primaryKey);
                priPubKeys.Add(publicKey);
            }
            return priPubKeys;
        }


    }
}
