using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DanbooruDownloader3.Entity
{
    [XmlType("tags")]
    public class DanbooruTags
    {
        [XmlElementAttribute("tag", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public tagsTag[] Tag { get; set; }

        private tagsTag[] generalTag;
        [XmlIgnore]
        public tagsTag[] GeneralTag
        {
            get
            {
                if (generalTag == null) generalTag = this.Tag.Where(x => x.Type == DanbooruTagType.General).ToArray<tagsTag>();
                return generalTag;
            }
            private set { }
        }

        private tagsTag[] artistTag;
        [XmlIgnore]
        public tagsTag[] ArtistTag
        {
            get
            {
                if (artistTag == null) artistTag = this.Tag.Where(x => x.Type == DanbooruTagType.Artist).ToArray<tagsTag>();
                return artistTag;
            }
            private set { }
        }

        private tagsTag[] copyrightTag;
        [XmlIgnore]
        public tagsTag[] CopyrightTag
        {
            get
            {
                if (copyrightTag == null) copyrightTag = this.Tag.Where(x => x.Type == DanbooruTagType.Copyright).ToArray<tagsTag>();
                return copyrightTag;
            }
            private set { }
        }

        private tagsTag[] characterTag;
        [XmlIgnore]
        public tagsTag[] CharacterTag
        {
            get
            {
                if (characterTag == null) characterTag = this.Tag.Where(x => x.Type == DanbooruTagType.Character).ToArray<tagsTag>();
                return characterTag;
            }
            private set { }
        }

        private tagsTag[] circleTag;
        [XmlIgnore]
        public tagsTag[] CircleTag
        {
            get
            {
                if (circleTag == null) circleTag = this.Tag.Where(x => x.Type == DanbooruTagType.Circle).ToArray<tagsTag>();
                return circleTag;
            }
            private set { }
        }

        private tagsTag[] faultsTag;
        [XmlIgnore]
        public tagsTag[] FaultsTag
        {
            get
            {
                if (faultsTag == null) faultsTag = this.Tag.Where(x => x.Type == DanbooruTagType.Faults).ToArray<tagsTag>();
                return faultsTag;
            }
            private set { }
        }

    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class tagsTag
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
        Faults = 6
    }
}