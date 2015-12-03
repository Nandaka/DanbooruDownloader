using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace DanbooruDownloader3.Entity
{
    public enum PreferredMethod
    {
        Xml,
        Json,
        Html
    }

    public enum BoardType
    {
        Danbooru,
        Gelbooru,
        Shimmie2
    }

    public enum LoginType
    {
        Anonymous,
        UserPass,
        Cookie
    }

    public class DanbooruProvider
    {
        // Remember to add the new member to DAO load action
        public string Name { get; set; }

        public int DefaultLimit { get; set; }

        public int HardLimit { get; set; }

        public PreferredMethod Preferred { get; set; }

        public string QueryStringXml { get; set; }

        public string QueryStringJson { get; set; }

        public string QueryStringHtml { get; set; }

        public string Url { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        //public bool UseAuth { get; set; }
        public LoginType LoginType { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public BoardType BoardType { get; set; }

        public bool TagDownloadUseLoop { get; set; }

        private bool _hasPrivateTags;

        [XmlIgnore]
        public bool HasPrivateTags
        {
            get
            {
                if (_danbooruTagCollection == null)
                {
                    LoadProviderTagCollection();
                }
                return _hasPrivateTags;
            }
            private set
            {
                _hasPrivateTags = value;
            }
        }

        private DanbooruTagCollection _danbooruTagCollection;

        [XmlIgnore]
        public DanbooruTagCollection ProviderTagCollection
        {
            get
            {
                if (_danbooruTagCollection == null)
                {
                    _danbooruTagCollection = LoadProviderTagCollection();
                }
                return _danbooruTagCollection;
            }

            private set { _danbooruTagCollection = value; }
        }

        public DanbooruTagCollection LoadProviderTagCollection()
        {
            try
            {
                DanbooruTagsDao dao = new DanbooruTagsDao("tags-" + Name + ".xml");
                _hasPrivateTags = true;
                return dao.Tags;
            }
            catch (FileNotFoundException)
            {
                _hasPrivateTags = false;
                return DanbooruTagsDao.Instance.Tags;
            }
        }

        public override string ToString()
        {
            //return "Name: " + Name +
            //    " Url: " + Url +
            //    " QueryJson: " + QueryStringJson +
            //    " QueryHtml: " + QueryStringHtml +
            //    " QueryXml: " + QueryStringXml +
            //    " Preferred: " + Preferred +
            //    " Default Limit: " + DefaultLimit +
            //    " Type: " + BoardType.ToString();
            return Name;
        }

        public string GetQueryString(DanbooruSearchParam searchParam)
        {
            var queryStr = "";
            if (this.BoardType == BoardType.Shimmie2)
            {
                queryStr = DanbooruDownloader3.Engine.ShimmieEngine.GetQueryString(this, searchParam);
            }
            else if (this.Url.Contains("sankakucomplex.com"))
            {
                queryStr = new SankakuComplexParser().GenerateQueryString(searchParam);
            }
            else
            {
                queryStr = DanbooruDownloader3.Engine.DanbooruXmlEngine.GetQueryString(this, searchParam);
            }

            return queryStr;
        }

        public string GetQueryUrl(DanbooruSearchParam searchParam)
        {
            var queryStr = GetQueryString(searchParam);

            var queryRootUrl = ""; ;
            switch (this.Preferred)
            {
                case PreferredMethod.Xml:
                    queryRootUrl = this.QueryStringXml;
                    break;

                case PreferredMethod.Json:
                    queryRootUrl = this.QueryStringJson;
                    break;

                case PreferredMethod.Html:
                    queryRootUrl = this.QueryStringHtml;
                    break;

                default:
                    break;
            }

            queryStr = queryRootUrl.Replace("%_query%", queryStr);

            switch (this.LoginType)
            {
                case Entity.LoginType.UserPass:
                    {
                        var hash = this.PasswordHash;
                        if (String.IsNullOrWhiteSpace(hash))
                        {
                            hash = Helper.GeneratePasswordHash(this.Password, this.PasswordSalt);
                            this.PasswordHash = hash;
                        }
                        string authString = "login=" + this.UserName + "&password_hash=" + hash;
                        queryStr = queryStr + "&" + authString;
                    }
                    break;

                case Entity.LoginType.Cookie:
                    // need to inject csv cookie  to the webclient
                    var cookies = Helper.ParseCookie(this.UserName, this.Url);
                    foreach (var cookie in cookies)
                    {
                        ExtendedWebClient.CookieJar.Add(cookie);
                    }
                    break;

                default:
                    break;
            }

            return this.Url + queryStr;
        }
    }
}