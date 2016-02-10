using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruTagsDao
    {
        public DanbooruTagCollection Tags { get; set; }

        public DanbooruTagsDao(string xmlTagFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DanbooruTagCollection));

            if (!File.Exists(xmlTagFile)) throw new FileNotFoundException("Cannot load tags.xml", xmlTagFile);
            using (StreamReader s = File.OpenText(xmlTagFile))
            {
                try
                {
                    this.Tags = (DanbooruTagCollection)ser.Deserialize(s);
                }
                catch (Exception ex)
                {
                    Program.Logger.Error("Failed to parse: " + xmlTagFile, ex);
                    this.Tags = new DanbooruTagCollection();
                }
            }
        }

        public bool IsArtistTag(string tag)
        {
            if (Tags.ArtistTag == null) return false;
            var result = Tags.ArtistTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCopyrightTag(string tag)
        {
            if (Tags.CopyrightTag == null) return false;
            var result = Tags.CopyrightTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCharacterTag(string tag)
        {
            if (Tags.CharacterTag == null) return false;
            var result = Tags.CharacterTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsCircleTag(string tag)
        {
            if (Tags.CircleTag == null) return false;
            var result = Tags.CircleTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public bool IsFaultsTag(string tag)
        {
            if (Tags.FaultsTag == null) return false;
            var result = Tags.FaultsTag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result == null) return false;
            return true;
        }

        public DanbooruTagType GetTagType(string tag)
        {
            if (Tags.Tag == null) return DanbooruTagType.General;
            var result = Tags.Tag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result != null) return result.Type;
            return DanbooruTagType.General;
        }

        public DanbooruTag GetTag(string tag)
        {
            if (Tags.Tag != null)
            {
                // TODO: Hot spot for perfomance
                var result = Tags.Tag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
                if (result != null) return result;
            }

            var unknownTag = new DanbooruTag()
            {
                Name = tag,
                Type = DanbooruTagType.Unknown,
                Count = -1,
                Id = "-1"
            };
            return unknownTag;
        }

        public DanbooruTag GetTag(string tag, DanbooruTagCollection tagCollection)
        {
            // TODO: Hot spot for perfomance
            var result = tagCollection.Tag.FirstOrDefault<DanbooruTag>(x => x.Name == tag);
            if (result != null) return result;
            else
            {
                var unknownTag = new DanbooruTag()
                {
                    Name = tag,
                    Type = DanbooruTagType.Unknown,
                    Count = -1,
                    Id = "-1"
                };
                return unknownTag;
            }
        }

        public List<DanbooruTag> ParseTagsString(string tagsStr)
        {
            List<DanbooruTag> tags = new List<DanbooruTag>();
            if (!String.IsNullOrWhiteSpace(tagsStr))
            {
                var tokens = tagsStr.Split(' ');
                foreach (var item in tokens)
                {
                    tags.Add(GetTag(item.Trim()));
                }
            }
            return tags;
        }

        public List<DanbooruTag> ParseTagsString(string tagsStr, DanbooruTagCollection tagCollection)
        {
            List<DanbooruTag> tags = new List<DanbooruTag>();
            var tokens = tagsStr.Split(' ');
            foreach (var item in tokens)
            {
                tags.Add(GetTag(item.Trim(), tagCollection));
            }
            return tags;
        }

        private static DanbooruTagsDao _DefaultTagsDao;

        public static DanbooruTagsDao Instance
        {
            get
            {
                if (_DefaultTagsDao == null)
                {
                    _DefaultTagsDao = new DanbooruTagsDao("tags.xml");
                }
                return _DefaultTagsDao;
            }
            set { _DefaultTagsDao = value; }
        }

        /// <summary>
        /// Update the source xml with the tags from target xml
        /// The updated tags is saved to target xml
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Merge(string source, string target)
        {
            if (!File.Exists(source)) return "Cannot find source";
            if (!File.Exists(target)) return "Cannot find target";

            int added = 0;
            int updated = 0;
            var sourceInstance = new DanbooruTagsDao(source).Tags.Tag.ToList();
            var targetInstance = new DanbooruTagsDao(target).Tags.Tag.ToList();

            foreach (var targetItem in targetInstance)
            {
                var sourceIndex = sourceInstance.FindIndex(x => x.Name == targetItem.Name);
                if (sourceIndex > 0)
                {
                    if (sourceInstance[sourceIndex].Type < targetItem.Type)
                    {
                        sourceInstance[sourceIndex].Type = targetItem.Type;
                        ++updated;
                    }
                }
                else
                {
                    sourceInstance.Add(targetItem);
                    ++added;
                }
            }

            Save(target, sourceInstance);
            return "Added: " + added + " tags, Updated: " + updated + " tags";
        }

        public static void Save(string target, List<DanbooruTag> sourceInstance)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DanbooruTagCollection));
            using (StreamWriter s = File.CreateText(target))
            {
                DanbooruTagCollection col = new DanbooruTagCollection();
                col.Tag = sourceInstance.ToArray();
                ser.Serialize(s, col);
            }
        }
    }
}