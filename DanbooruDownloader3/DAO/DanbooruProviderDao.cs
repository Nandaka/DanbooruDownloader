using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3.DAO
{
    class DanbooruProviderDao
    {
        string xmlProviderUrl = @"DanbooruProviderList.xml";

        public List<DanbooruProvider> GetAllProvider()
        {
            List<DanbooruProvider> list = new List<DanbooruProvider>();

            XmlTextReader reader = new XmlTextReader(xmlProviderUrl);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals("DanbooruProviderList")) break;
                        else if (reader.Name.Equals("DanbooruProvider"))
                        {
                            Console.WriteLine(reader.Value);
                            
                            DanbooruProvider newProvider = new DanbooruProvider();
                            do
                            {
                                reader.Read();
                                if (reader.NodeType.Equals(XmlNodeType.Element))
                                {
                                    switch (reader.Name)
                                    {
                                        case "Name": newProvider.Name = reader.ReadElementContentAsString(); break;
                                        case "Url": newProvider.Url = reader.ReadElementContentAsString(); break;
                                        case "QueryStringJson": newProvider.QueryStringJson = reader.ReadElementContentAsString(); break;
                                        case "QueryStringXml": newProvider.QueryStringXml = reader.ReadElementContentAsString(); break;
                                        case "Preferred": string preferred = reader.ReadElementContentAsString(); newProvider.Preferred = preferred.Equals("Json") ? PreferredMethod.Json:PreferredMethod.Xml ; break;
                                        case "DefaultLimit": newProvider.DefaultLimit = reader.ReadElementContentAsInt(); break;
                                        default: break;
                                    }
                                }
                            } while (!reader.Name.Equals("DanbooruProvider"));
                            list.Add(newProvider);
                        }
                        break;
                    default: break;
                }
            }
            return list;
        }
    }
}
