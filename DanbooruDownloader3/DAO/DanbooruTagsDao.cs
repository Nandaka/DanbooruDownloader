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
        public DanbooruTags Tags { get; set; }
        public DanbooruTagsDao(string xmlTagFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DanbooruTags));
            this.Tags = (DanbooruTags)ser.Deserialize(File.OpenText(xmlTagFile));
        }

        public bool IsArtistTag(string tag)
        {
            var result = Tags.ArtistTag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCopyrightTag(string tag)
        {
            var result = Tags.CopyrightTag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCharacterTag(string tag)
        {
            var result = Tags.CharacterTag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCircleTag(string tag)
        {
            var result = Tags.CircleTag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsFaultsTag(string tag)
        {
            var result = Tags.FaultsTag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public DanbooruTagType GetTagType(string tag)
        {
            var result = Tags.Tag.FirstOrDefault<tagsTag>(x => x.Name == tag);
            if (result != null) return result.Type;
            return DanbooruTagType.General;
        }
    }
}
