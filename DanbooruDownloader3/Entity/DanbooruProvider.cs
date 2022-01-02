using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using System;
using System.IO;
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
        Cookie,
        CookieAlwaysAsk
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

        public LoginType LoginType { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public BoardType BoardType { get; set; }

        public bool TagDownloadUseLoop { get; set; }

        public string DateTimeFormat { get; set; }

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
                if (_hasPrivateTags)
                {
                    DanbooruTagsDao dao = new DanbooruTagsDao($"tags-{Name}.xml");
                    _hasPrivateTags = true;
                    return dao.Tags;
                }
            }
            catch (FileNotFoundException)
            {
                _hasPrivateTags = false;
            }
            return DanbooruTagsDao.Instance.Tags;
        }

        public override string ToString()
        {
            return Name;
        }

        public string GetQueryString(DanbooruSearchParam searchParam)
        {
            var queryStr = "";
            if (BoardType == BoardType.Shimmie2)
            {
                if (Preferred == PreferredMethod.Html)
                {
                    queryStr = new ShimmieHtmlParser().GenerateQueryString(searchParam);
                }
                else
                {
                    queryStr = ShimmieEngine.GetQueryString(this, searchParam);
                }
            }
            else if (Url.Contains("sankakucomplex.com"))
            {
                queryStr = new SankakuComplexParser().GenerateQueryString(searchParam);
            }
            else
            {
                queryStr = DanbooruXmlEngine.GetQueryString(this, searchParam);
            }

            return queryStr;
        }

        public string GetQueryUrl(DanbooruSearchParam searchParam)
        {
            var queryStr = GetQueryString(searchParam);

            var queryRootUrl = ""; ;
            switch (Preferred)
            {
                case PreferredMethod.Xml:
                    queryRootUrl = QueryStringXml;
                    break;

                case PreferredMethod.Json:
                    queryRootUrl = QueryStringJson;
                    break;

                case PreferredMethod.Html:
                    queryRootUrl = QueryStringHtml;
                    break;

                default:
                    break;
            }

            queryStr = queryRootUrl.Replace("%_query%", queryStr);

            switch (LoginType)
            {
                case LoginType.UserPass:
                    {
                        var hash = PasswordHash;
                        if (String.IsNullOrWhiteSpace(hash))
                        {
                            hash = Helper.GeneratePasswordHash(Password, PasswordSalt);
                            PasswordHash = hash;
                        }
                        queryStr = $"{queryStr}&login={UserName}&password_hash={hash}";
                    }
                    break;

                case LoginType.Cookie:
                case LoginType.CookieAlwaysAsk:
                    // need to inject csv cookie to the webclient
                    ReloadCookie(UserName);
                    break;

                default:
                    break;
            }

            return $"{Url}{queryStr}";
        }

        public void ReloadCookie(string newCookie)
        {
            UserName = newCookie;
            var cookies = Helper.ParseCookie(newCookie, Url);
            foreach (var cookie in cookies)
            {
                ExtendedWebClient.CookieJar.Add(cookie);
            }
        }
    }
}