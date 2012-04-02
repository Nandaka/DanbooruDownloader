using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.Reflection;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruProviderDao
    {
        public List<DanbooruProvider> Read(string filename = @"DanbooruProviderList.xml")
        {
            List<DanbooruProvider> list = new List<DanbooruProvider>();

            using (XmlTextReader reader = new XmlTextReader(filename))
            {
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
                                            case "Preferred":
                                                string preferred = reader.ReadElementContentAsString();
                                                newProvider.Preferred = preferred.Equals("Json") ? PreferredMethod.Json : PreferredMethod.Xml; break;
                                            case "DefaultLimit": newProvider.DefaultLimit = reader.ReadElementContentAsInt(); break;
                                            case "HardLimit": newProvider.HardLimit = reader.ReadElementContentAsInt(); break;
                                            case "UserName": newProvider.UserName = reader.ReadElementContentAsString(); break;
                                            case "Password": newProvider.Password = reader.ReadElementContentAsString(); break;
                                            case "UseAuth": newProvider.UseAuth = reader.ReadElementContentAsBoolean(); break;
                                            case "PasswordSalt": newProvider.PasswordSalt = reader.ReadElementContentAsString(); break;
                                            case "BoardType":
                                                string type = reader.ReadElementContentAsString();
                                                newProvider.BoardType = (BoardType)Enum.Parse(typeof(BoardType), type); //Type.Equals("Danbooru") ? BoardType.Danbooru:BoardType.Gelbooru ; 
                                                break;
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
            }
            return list;
        }

        public void Save(List<DanbooruProvider> list, string filename = @"DanbooruProviderList.xml")
        {
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Indent = true;
            
            using (XmlWriter writer = XmlWriter.Create(filename, setting))
            {                
                writer.WriteStartDocument();
                writer.WriteStartElement("DanbooruProviderList");
                foreach (DanbooruProvider p in list)
                {
                    writer.WriteStartElement("DanbooruProvider");
                    PropertyInfo[] propertyInfos;
                    propertyInfos = typeof(DanbooruProvider).GetProperties();
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        if (info.PropertyType.Name == "Boolean")
                        {
                            writer.WriteElementString(info.Name, info.GetValue(p, null).ToString().ToLowerInvariant());
                        }
                        else
                        {
                            writer.WriteElementString(info.Name, info.GetValue(p, null).ToString());
                        }
                    }

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

    }
}
