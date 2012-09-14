using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using DanbooruDownloader3.Entity;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DanbooruDownloader3
{
    public static class Helper
    {
        public static Color ColorGeneral = Color.Black;
        public static Color ColorArtist = Color.HotPink;
        public static Color ColorCopyright = Color.OrangeRed;
        public static Color ColorCharacter = Color.Blue;
        public static Color ColorCircle = Color.Purple;
        public static Color ColorFaults = Color.Red;
        public static Color ColorBlacklisted = Color.LightGray;

        /// <summary>
        /// Generate hashed password+salt using SHA1
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GeneratePasswordHash(string password, string salt)
        {
            string hash = "";
            string temp = salt.Replace("%PASSWORD%", password);

            byte[] result = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(temp));

            foreach (byte b in result)
            {
                string hexValue = b.ToString("X").ToLower(); // Lowercase for compatibility on case-sensitive systems
                hash += (hexValue.Length == 1 ? "0" : "") + hexValue;
            }

            return hash;
        }

        /// <summary>
        /// Pad user agent with current date/time.
        /// </summary>
        /// <param name="originalUserAgent"></param>
        /// <returns></returns>
        public static string PadUserAgent(string originalUserAgent)
        {
            if (originalUserAgent != null && originalUserAgent.Length > 0)
            {
                return originalUserAgent + DateTime.UtcNow;
            }
            else return null;
        }

        /// <summary>
        /// Sanitize the TAGS_FILENAME.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeFilename(string input)
        {
            if (input == null) return "";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            input = input.Replace(':', '_');
            input = input.Replace('\\', '_');
            return input;
        }

        /// <summary>
        /// For constructing TAGS_FILENAME.
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="format"></param>
        /// <param name="post"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static string MakeFilename(DanbooruFilenameFormat format, DanbooruPost post)
        {
            string filename = format.FilenameFormat;
            string provider = post.Provider;
            string query = post.Query;
            string searchTags = post.SearchTags;
            string originalFileName = post.FileUrl.Split('/').Last();

            filename = filename.Replace("%provider%", Helper.SanitizeFilename(provider));
            filename = filename.Replace("%id%", post.Id);
            filename = filename.Replace("%rating%", post.Rating);
            filename = filename.Replace("%md5%", post.MD5);
            filename = filename.Replace("%query%", Helper.SanitizeFilename(query));
            filename = filename.Replace("%searchtag%", Helper.SanitizeFilename(searchTags));
            filename = filename.Replace("%originalFilename%", Helper.SanitizeFilename(originalFileName));

            // copy the tags entity to be grouped.
            var groupedTags = post.TagsEntity;

            var artistSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Artist).Select(x => x.Name);
            var artist = "";
            if (artistSelection != null)
            {
                if (artistSelection.Count() >= format.ArtistGroupLimit && !string.IsNullOrWhiteSpace(format.ArtistGroupReplacement))
                {
                    artist = format.ArtistGroupReplacement;
                    groupedTags.RemoveAll(x => x.Type == DanbooruTagType.Artist);
                    groupedTags.Add(new DanbooruTag() { Name = format.ArtistGroupReplacement });
                }
                else
                {
                    artist = string.Join(" ", artistSelection);
                }
            }
            if (string.IsNullOrWhiteSpace(artist)) artist = format.MissingTagReplacement;
            filename = filename.Replace("%artist%", Helper.SanitizeFilename(artist).Trim());

            var copyrightSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Copyright).Select(x => x.Name);
            var copyright = "";
            if (copyrightSelection != null)
            {
                if (copyrightSelection.Count() >= format.CopyrightGroupLimit && 
                    format.CopyrightGroupLimit > 1 &&
                    !string.IsNullOrWhiteSpace(format.CopyrightGroupReplacement))
                {
                    copyright = format.CopyrightGroupReplacement;
                    groupedTags.RemoveAll(x => x.Type == DanbooruTagType.Artist);
                    groupedTags.Add(new DanbooruTag() { Name = format.ArtistGroupReplacement });
                }
                else
                {
                    copyright = string.Join(" ", copyrightSelection);
                }
            }
            if (string.IsNullOrWhiteSpace(copyright)) copyright = format.MissingTagReplacement;
            filename = filename.Replace("%copyright%", Helper.SanitizeFilename(copyright.Trim()));

            var characterSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Character).Select(x => x.Name);
            var character = "";
            if (characterSelection != null)
            {
                if (characterSelection.Count() >= format.CharacterGroupLimit &&
                    format.CharacterGroupLimit > 1 &&
                    !string.IsNullOrWhiteSpace(format.CharacterGroupReplacement))
                {
                    character = format.CharacterGroupReplacement;
                    groupedTags.RemoveAll(x => x.Type == DanbooruTagType.Character);
                    groupedTags.Add(new DanbooruTag() { Name = format.CharacterGroupReplacement });
                }
                else
                {
                    character = string.Join(" ", characterSelection);
                }
            }
            if (string.IsNullOrWhiteSpace(character)) character = format.MissingTagReplacement;
            filename = filename.Replace("%character%", Helper.SanitizeFilename(character.Trim()));
            
            var circleSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Circle).Select(x => x.Name);
            var circle = "";
            if (circleSelection != null)
            {
                if (circleSelection.Count() >= format.CircleGroupLimit &&
                    format.CircleGroupLimit > 1 &&
                    !string.IsNullOrWhiteSpace(format.CircleGroupReplacement))
                {
                    circle = format.CircleGroupReplacement;
                    groupedTags.RemoveAll(x => x.Type == DanbooruTagType.Circle);
                    groupedTags.Add(new DanbooruTag() { Name = format.CircleGroupReplacement });
                }
                else
                {
                    circle = string.Join(" ", circleSelection);
                }
            }
            if (string.IsNullOrWhiteSpace(circle)) circle = format.MissingTagReplacement;
            filename = filename.Replace("%circle%", Helper.SanitizeFilename(circle.Trim()));
            
            var faultsSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Faults).Select(x => x.Name);
            var faults = "";
            if (faultsSelection != null)
            {
                if (faultsSelection.Count() >= format.FaultsGroupLimit &&
                    format.FaultsGroupLimit > 1 &&
                    !string.IsNullOrWhiteSpace(format.FaultsGroupReplacement))
                {
                    faults = format.FaultsGroupReplacement;
                    groupedTags.RemoveAll(x => x.Type == DanbooruTagType.Faults);
                    groupedTags.Add(new DanbooruTag() { Name = format.FaultsGroupReplacement });
                }
                else
                {
                    faults = string.Join(" ", faultsSelection);
                }
            }
            if (string.IsNullOrWhiteSpace(faults)) faults = format.MissingTagReplacement;
            filename = filename.Replace("%faults%", Helper.SanitizeFilename(faults.Trim()));

            groupedTags.Sort();
            filename = filename.Replace("%tags%", Helper.SanitizeFilename(string.Join(" ", groupedTags.Select(x=> x.Name))));

            // append base folder from Save Folder text box
            if (format.BaseFolder.EndsWith(@"\")) filename = format.BaseFolder + filename;
            else if (!String.IsNullOrWhiteSpace(format.BaseFolder)) filename = format.BaseFolder + @"\" + filename;

            filename = filename.Substring(0, filename.Length < format.Limit ? filename.Length : format.Limit).Trim();

            // check if contains subdirectory
            if (filename.Contains(@"\"))
            {
                string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }

            return filename;
        }

        /// <summary>
        /// Decode JSON Encoded Unicode Character to C# string.
        /// http://stackoverflow.com/questions/1615559/converting-unicode-strings-to-escaped-ascii-string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        /// <summary>
        /// Replace password_hash param with '*'
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RemoveAuthInfo(string url)
        {            
            if (!string.IsNullOrWhiteSpace(url) && url.Contains("password_hash="))
            {
                var splitted = url.Split('&');
                for (int i = 0; i < splitted.Length; ++i)
                {
                    if (splitted[i].StartsWith("password_hash="))
                    {
                        var passSplit = splitted[i].Split('=');
                        if (passSplit.Length == 2)
                        {
                            passSplit[1] = "".PadLeft(passSplit[1].Length, '*');
                        }
                        splitted[i] = String.Join("=", passSplit);
                    }
                }
                return String.Join("&", splitted);
            }
            else return url;
        }

        public static bool IsTagsXmlExist()
        {
            return File.Exists("tags.xml");
        }
    }
}
