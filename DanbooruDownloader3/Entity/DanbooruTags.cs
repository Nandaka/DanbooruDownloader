using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System;

namespace DanbooruDownloader3.Entity
{
    [XmlType("tags")]
    public class DanbooruTagCollection
    {
        [XmlElementAttribute("tag", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DanbooruTag[] Tag { get; set; }

        private DanbooruTag[] generalTag;
        [XmlIgnore]
        public DanbooruTag[] GeneralTag
        {
            get
            {
                if (generalTag == null) generalTag = this.Tag.Where(x => x.Type == DanbooruTagType.General).ToArray<DanbooruTag>();
                return generalTag;
            }
            private set { }
        }

        private DanbooruTag[] artistTag;
        [XmlIgnore]
        public DanbooruTag[] ArtistTag
        {
            get
            {
                if (artistTag == null) artistTag = this.Tag.Where(x => x.Type == DanbooruTagType.Artist).ToArray<DanbooruTag>();
                return artistTag;
            }
            private set { }
        }

        private DanbooruTag[] copyrightTag;
        [XmlIgnore]
        public DanbooruTag[] CopyrightTag
        {
            get
            {
                if (copyrightTag == null) copyrightTag = this.Tag.Where(x => x.Type == DanbooruTagType.Copyright).ToArray<DanbooruTag>();
                return copyrightTag;
            }
            private set { }
        }

        private DanbooruTag[] characterTag;
        [XmlIgnore]
        public DanbooruTag[] CharacterTag
        {
            get
            {
                if (characterTag == null) characterTag = this.Tag.Where(x => x.Type == DanbooruTagType.Character).ToArray<DanbooruTag>();
                return characterTag;
            }
            private set { }
        }

        private DanbooruTag[] circleTag;
        [XmlIgnore]
        public DanbooruTag[] CircleTag
        {
            get
            {
                if (circleTag == null) circleTag = this.Tag.Where(x => x.Type == DanbooruTagType.Circle).ToArray<DanbooruTag>();
                return circleTag;
            }
            private set { }
        }

        private DanbooruTag[] faultsTag;
        [XmlIgnore]
        public DanbooruTag[] FaultsTag
        {
            get
            {
                if (faultsTag == null) faultsTag = this.Tag.Where(x => x.Type == DanbooruTagType.Faults).ToArray<DanbooruTag>();
                return faultsTag;
            }
            private set { }
        }

        public override string ToString()
        {
            return "Tags Count: " + Tag.Length;
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class DanbooruTag : IComparable<DanbooruTag>
    {
        [XmlAttribute("type")]
        public DanbooruTagType Type { get; set; }

        [XmlAttribute("ambiguous")]
        public bool Ambiguous { get; set; }

        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        #region IComparable Members

        public int CompareTo(DanbooruTag obj)
        {
            return string.Compare(this.Name, obj.Name);
        }

        #endregion
    }

    public enum DanbooruTagType
    {
        [XmlEnum(Name = "0")]
        General = 0,
        [XmlEnum(Name = "1")]
        Artist = 1,
        [XmlEnum(Name = "3")]
        Copyright = 3,
        [XmlEnum(Name = "4")]
        Character = 4,
        [XmlEnum(Name = "5")]
        Circle = 5,
        [XmlEnum(Name = "6")]
        Faults = 6,
        [XmlEnum(Name = "-1")]
        Unknown = -1
    }
}