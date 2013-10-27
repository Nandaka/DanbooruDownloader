using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DanbooruDownloader3.DAO;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

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
        public bool UseAuth { get; set; }
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
    }
}
