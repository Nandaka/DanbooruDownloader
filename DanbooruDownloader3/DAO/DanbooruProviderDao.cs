using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruProviderDao
    {
        public const string DATETIME_FORMAT_UNIX = "unix-timestamp";
        public const string DATETIME_FORMAT_NA = "not-available";

        private static DanbooruProviderDao _instance;

        public static DanbooruProviderDao GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DanbooruProviderDao();
            }
            return _instance;
        }

        private DanbooruProviderDao()
        {
        }

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
                                            case "QueryStringHtml": newProvider.QueryStringHtml = reader.ReadElementContentAsString(); break;
                                            case "QueryStringXml": newProvider.QueryStringXml = reader.ReadElementContentAsString(); break;
                                            case "Preferred":
                                                string preferred = reader.ReadElementContentAsString().ToLowerInvariant();
                                                switch (preferred)
                                                {
                                                    case "json":
                                                        newProvider.Preferred = PreferredMethod.Json;
                                                        break;

                                                    case "xml":
                                                        newProvider.Preferred = PreferredMethod.Xml;
                                                        break;

                                                    case "html":
                                                        newProvider.Preferred = PreferredMethod.Html;
                                                        break;

                                                    default:
                                                        throw new Exception("Invalid Provider Type in DanbooruProviderList.xml: " + preferred);
                                                }
                                                break;

                                            case "DefaultLimit": newProvider.DefaultLimit = reader.ReadElementContentAsInt(); break;
                                            case "HardLimit": newProvider.HardLimit = reader.ReadElementContentAsInt(); break;
                                            case "UserName": newProvider.UserName = reader.ReadElementContentAsString(); break;
                                            case "Password": newProvider.Password = reader.ReadElementContentAsString(); break;
                                            case "UseAuth":
                                                // compat
                                                var isAuth = reader.ReadElementContentAsBoolean();
                                                if (isAuth)
                                                {
                                                    newProvider.LoginType = LoginType.UserPass;
                                                }
                                                break;

                                            case "LoginType":
                                                string ltype = reader.ReadElementContentAsString();
                                                newProvider.LoginType = (LoginType)Enum.Parse(typeof(LoginType), ltype);
                                                break;

                                            case "PasswordSalt": newProvider.PasswordSalt = reader.ReadElementContentAsString(); break;
                                            case "PasswordHash": newProvider.PasswordHash = reader.ReadElementContentAsString(); break;
                                            case "BoardType":
                                                string type = reader.ReadElementContentAsString();
                                                newProvider.BoardType = (BoardType)Enum.Parse(typeof(BoardType), type); //Type.Equals("Danbooru") ? BoardType.Danbooru:BoardType.Gelbooru ;
                                                break;

                                            case "TagDownloadUseLoop": newProvider.TagDownloadUseLoop = reader.ReadElementContentAsBoolean(); break;
                                            case "DateTimeFormat": newProvider.DateTimeFormat = reader.ReadElementContentAsString(); break;
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
                        // skip private setter
                        if (!info.CanWrite || info.GetSetMethod() == null)
                        {
                            continue;
                        }
                        var value = info.GetValue(p, null);
                        if (value == null) value = "";
                        if (info.Name == "Password") value = ""; //Blank the password.

                        if (info.PropertyType.Name == "Boolean")
                        {
                            writer.WriteElementString(info.Name, value.ToString().ToLowerInvariant());
                        }
                        else
                        {
                            writer.WriteElementString(info.Name, value.ToString());
                        }
                    }

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
        }
    }
}