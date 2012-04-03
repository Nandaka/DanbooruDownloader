using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DanbooruDownloader3.Entity;
using System.Xml.Serialization;
using System.IO;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruTagsDao
    {
        public DanbooruTagCollection Tags { get; set; }
        public DanbooruTagsDao(string xmlTagFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DanbooruTagCollection));
            if (!File.Exists(xmlTagFile)) throw new FileNotFoundException("Cannot load tags.xml", xmlTagFile);
            this.Tags = (DanbooruTagCollection)ser.Deserialize(File.OpenText(xmlTagFile));
        }

        public bool IsArtistTag(string tag)
        {
            var result = Tags.ArtistTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCopyrightTag(string tag)
        {
            var result = Tags.CopyrightTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCharacterTag(string tag)
        {
            var result = Tags.CharacterTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCircleTag(string tag)
        {
            var result = Tags.CircleTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsFaultsTag(string tag)
        {
            var result = Tags.FaultsTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public DanbooruTagType GetTagType(string tag)
        {
            var result = Tags.Tag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result != null) return result.Type;
            return DanbooruTagType.General;
        }

        public DanbooruTag GetTag(string tag)
        {
            var result = Tags.Tag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result != null) return result;
            else
            {
                var unknownTag = new DanbooruTag()
                {
                    Name = tag, Type = DanbooruTagType.General, Count = -1, Id = "-1"
                };
                return unknownTag;
            }
        }

        public List<DanbooruTag> ParseTagsString(string tagsStr)
        {
            List<DanbooruTag> tags = new List<DanbooruTag>();
            var tokens = tagsStr.Split(' ');
            foreach (var item in tokens)
            {
                tags.Add(GetTag(item.Trim()));
            }
            return tags;
        }
    }
}
