using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
        public static Color ColorUnknown = Color.Gray;
        public static Color ColorDeleted = Color.FromArgb(255, 241, 243, 244);

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
                return originalUserAgent + DateTime.UtcNow.Ticks;
            }
            else return null;
        }

        /// <summary>
        /// Sanitize the TAGS_FILENAME.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeFilename(string input, bool allowPathSeparator = false)
        {
            if (input == null) return "";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (allowPathSeparator && (c == '\\' || c == '/'))
                    continue;
                input = input.Replace(c, '_');
            }
            input = input.Replace(':', '_');
            if (!allowPathSeparator)
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
            string provider = post.Provider.Name;
            string query = post.Query;
            string searchTags = post.SearchTags;
            string originalFileName = post.FileUrl.Split('/').Last();

            // sanitizing the format
            filename = Helper.SanitizeFilename(filename, true);

            //remove extension
            originalFileName = originalFileName.Substring(0, originalFileName.LastIndexOf('.'));
            originalFileName = Uri.UnescapeDataString(originalFileName);

            filename = filename.Replace("%provider%", Helper.SanitizeFilename(provider));
            filename = filename.Replace("%id%", post.Id);
            filename = filename.Replace("%rating%", post.Rating);
            filename = filename.Replace("%md5%", post.MD5);
            filename = filename.Replace("%searchParam%", Helper.SanitizeFilename(query));
            filename = filename.Replace("%searchtag%", Helper.SanitizeFilename(searchTags));
            filename = filename.Replace("%originalFilename%", Helper.SanitizeFilename(originalFileName));

            // copy the tags entity to be grouped.
            var groupedTags = post.TagsEntity;
            groupedTags.Sort();

            // remove ignored tags
            groupedTags = RemoveIgnoredTags(format, groupedTags);

            // artist
            var artist = FilterTags(post,
                                    groupedTags,
                                    DanbooruTagType.Artist,
                                    format.ArtistGroupLimit,
                                    format.ArtistGroupReplacement,
                                    format.MissingTagReplacement,
                                    format.IsReplaceMode,
                                    format.TagReplaceUnderscoreToSpace);
            var artistStr = Helper.SanitizeFilename(artist).Trim();
            if (String.IsNullOrEmpty(artistStr)) artistStr = DanbooruDownloader3.Properties.Settings.Default.tagNoArtistValue;
            filename = filename.Replace("%artist%", artistStr);

            // copyright
            var copyright = FilterTags(post,
                                       groupedTags,
                                       DanbooruTagType.Copyright,
                                       format.CopyrightGroupLimit,
                                       format.CopyrightGroupReplacement,
                                       format.MissingTagReplacement,
                                       format.IsReplaceMode,
                                       format.TagReplaceUnderscoreToSpace);
            var copyStr = Helper.SanitizeFilename(copyright.Trim());
            if (String.IsNullOrEmpty(copyStr)) copyStr = DanbooruDownloader3.Properties.Settings.Default.tagNoCopyrightValue;
            filename = filename.Replace("%copyright%", copyStr);

            // character
            var character = FilterTags(post,
                                       groupedTags,
                                       DanbooruTagType.Character,
                                       format.CharacterGroupLimit,
                                       format.CharacterGroupReplacement,
                                       format.MissingTagReplacement,
                                       format.IsReplaceMode,
                                       format.TagReplaceUnderscoreToSpace);
            var charaStr = Helper.SanitizeFilename(character.Trim());
            if (String.IsNullOrEmpty(charaStr)) charaStr = DanbooruDownloader3.Properties.Settings.Default.tagNoCharaValue;
            filename = filename.Replace("%character%", charaStr);

            // cirle
            var circleSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Circle).Select(x => x.Name);
            var circle = FilterTags(post,
                                    groupedTags,
                                    DanbooruTagType.Circle,
                                    format.CircleGroupLimit,
                                    format.CircleGroupReplacement,
                                    format.MissingTagReplacement,
                                    format.IsReplaceMode,
                                    format.TagReplaceUnderscoreToSpace);
            var circleStr = Helper.SanitizeFilename(circle.Trim());
            if (String.IsNullOrEmpty(circleStr)) circleStr = DanbooruDownloader3.Properties.Settings.Default.tagNoCircleValue;
            filename = filename.Replace("%circle%", circleStr);

            // faults
            var faultsSelection = post.TagsEntity.Where<DanbooruTag>(x => x.Type == DanbooruTagType.Faults).Select(x => x.Name);
            var faults = FilterTags(post,
                                    groupedTags,
                                    DanbooruTagType.Faults,
                                    format.FaultsGroupLimit,
                                    format.FaultsGroupReplacement,
                                    format.MissingTagReplacement,
                                    format.IsReplaceMode,
                                    format.TagReplaceUnderscoreToSpace);
            var faultStr = Helper.SanitizeFilename(faults.Trim());
            if (String.IsNullOrEmpty(faultStr)) faultStr = DanbooruDownloader3.Properties.Settings.Default.tagNoFaultValue;
            filename = filename.Replace("%faults%", faultStr);

            // all tags
            var allTempTags = groupedTags.Select(x => x.Name).ToList();
            if (format.TagReplaceUnderscoreToSpace)
            {
                for (int i = 0; i < allTempTags.Count; i++)
                {
                    allTempTags[i] = allTempTags[i].Replace("_", " ").Trim();
                }
            }
            filename = filename.Replace("%tags%", Helper.SanitizeFilename(string.Join(" ", allTempTags)));

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

        private static List<DanbooruTag> RemoveIgnoredTags(DanbooruFilenameFormat format, List<DanbooruTag> groupedTags)
        {
            foreach (DanbooruTag ignoredTag in format.IgnoredTags)
            {
                if (format.IgnoreTagsUseRegex)
                {
                    if (!String.IsNullOrWhiteSpace(format.IgnoredTagsRegex))
                    {
                        Regex re = new Regex(format.IgnoredTagsRegex, RegexOptions.IgnoreCase);

                        if (format.IgnoredTagsOnlyForGeneral)
                        {
                            groupedTags.RemoveAll(x => x.Type == DanbooruTagType.General && re.IsMatch(x.Name));
                        }
                        else
                        {
                            groupedTags.RemoveAll(x => re.IsMatch(x.Name));
                        }
                    }
                }
                else
                {
                    groupedTags.RemoveAll(x => x.Name == ignoredTag.Name);
                }
            }

            return groupedTags;
        }

        private static string FilterTags(DanbooruPost post,
                                         List<DanbooruTag> groupedTags,
                                         DanbooruTagType tagType,
                                         int tagLimit,
                                         string tagReplacement,
                                         string missingTagReplacement,
                                         bool isReplaceMode,
                                         bool isReplaceUnderScore)
        {
            var selectedTags = post.TagsEntity.Where<DanbooruTag>(x => x.Type == tagType).Select(x => x.Name).ToList();
            var tagStr = "";
            if (selectedTags != null)
            {
                if (isReplaceUnderScore)
                {
                    for (int i = 0; i < selectedTags.Count; i++)
                    {
                        selectedTags[i] = selectedTags[i].Replace("_", " ").Trim();
                    }
                }
                if (selectedTags.Count() >= tagLimit)
                {
                    if (isReplaceMode && !string.IsNullOrWhiteSpace(tagReplacement))
                    {
                        tagStr = tagReplacement;
                        if (isReplaceUnderScore)
                            tagStr = tagReplacement.Replace("_", " ").Trim();
                        groupedTags.RemoveAll(x => x.Type == tagType);
                        groupedTags.Add(new DanbooruTag() { Name = tagReplacement });
                    }
                    else
                    {
                        var tempTags = selectedTags.Take(tagLimit);
                        tagStr = String.Join(" ", tempTags);
                    }
                }
                else
                {
                    tagStr = string.Join(" ", selectedTags);
                }
            }

            if (string.IsNullOrWhiteSpace(tagStr)) tagStr = missingTagReplacement;
            return tagStr;
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

        public static bool DumpRawData(string data, string filename)
        {
            try
            {
                using (StreamWriter output = File.CreateText(filename))
                {
                    output.Write(data);
                    output.Flush();
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.Logger.Error("Failed to create dump file: " + filename, ex);
                return false;
            }
        }

        public static bool DumpRawData(string data, DanbooruProvider provider, string query)
        {
            string filename = "Dump for List " + provider.Name + " Query " + query + ".txt";
            filename = Helper.SanitizeFilename(filename);
            try
            {
                using (StreamWriter output = File.CreateText(filename))
                {
                    output.Write(data);
                    output.Flush();
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.Logger.Error("Failed to create dump file: " + filename, ex);
                return false;
            }
        }

        public static bool CheckBlacklistedTag(DanbooruPost post, DanbooruPostDaoOption option)
        {
            if (option.BlacklistedTagsUseRegex)
            {
                if (option.IsBlacklistOnlyForGeneral)
                {
                    return post.TagsEntity.Any(x => x.Type == DanbooruTagType.General && option.BlacklistedTagsRegex.IsMatch(x.Name));
                }
                else return post.TagsEntity.Any(x => option.BlacklistedTagsRegex.IsMatch(x.Name));
            }
            else
            {
                if (option.IsBlacklistOnlyForGeneral)
                {
                    foreach (var tag in option.BlacklistedTags)
                    {
                        if (post.TagsEntity.Any(x => x.Type == DanbooruTagType.General && x.Name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase)))
                            return true;
                    }
                }
                else
                {
                    foreach (var tag in option.BlacklistedTags)
                    {
                        if (post.TagsEntity.Any(x => x.Name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase)))
                            return true;
                    }
                }
            }
            return false;
        }

        public static string FormatByteSize(long size)
        {
            double sizeD = size;
            if (size < 1024)
                return String.Format("{0} bytes", size);
            else
            {
                sizeD = sizeD / 1024;
                if (sizeD >= 1 && sizeD < 1024)
                    return String.Format("{0:F2} Kbytes", sizeD);
                sizeD = sizeD / 1024;
                if (sizeD >= 1 && sizeD < 1024)
                    return String.Format("{0:F2} Mbytes", sizeD);
                sizeD = sizeD / 1024;
                return String.Format("{0:F2} Gbytes", sizeD);
            }
        }

        public static List<DanbooruTag> ParseTags(string p, DanbooruProvider provider)
        {
            if (provider != null && provider.HasPrivateTags && !DanbooruDownloader3.Properties.Settings.Default.UseGlobalProviderTags)
            {
                return DanbooruTagsDao.Instance.ParseTagsString(p, provider.ProviderTagCollection);
            }
            else
                return DanbooruTagsDao.Instance.ParseTagsString(p);
        }

        /// <summary>
        /// Remove control char from unicode string
        /// http://stackoverflow.com/a/6799681
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];

                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }

        /// <summary>
        /// http://stackoverflow.com/a/14488941
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static string shortSpeedStr(long speed)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int i = 0;
            decimal dValue = (decimal)speed;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, suffixes[i]);
        }

        public static string getFileExtensions(string url)
        {
            // http://cs.sankakucomplex.com/data/4c/87/4c87c7e5fbda5e0c2eda1bd0797a008e.jpg?136708
            string extension = url.Substring(url.LastIndexOf('.'));
            if (extension.Contains("?"))
            {
                extension = extension.Substring(0, extension.LastIndexOf("?"));
            }

            if (Properties.Settings.Default.RenameJpeg)
            {
                if (extension.EndsWith(".jpeg")) extension = ".jpg";
            }
            return extension;
        }

        public static string FixUrl(string url, bool useHttps = false, string hostname = null)
        {
            if (String.IsNullOrWhiteSpace(url)) return url;

            // prefix with http/https if start without protocol
            // e.g.: //example.com/something.jpg
            if (url.StartsWith("//"))
            {
                if (useHttps)
                    url = "https:" + url;
                else
                    url = "http:" + url;
            }
            // relative url, append with hostname
            // e.g.: /some/relative/url.jpg
            else if (url.StartsWith("/") && hostname != null)
            {
                if (!hostname.StartsWith("http"))
                {
                    if (useHttps)
                        hostname = "https://" + hostname;
                    else
                        hostname = "http://" + hostname;
                }
                url = hostname + url;
            }

            return url;
        }

        public static void WriteTextFile(string content, string filename = null)
        {
            if (String.IsNullOrWhiteSpace(filename))
                filename = String.Format("Batch Download on {0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));

            if (File.Exists(filename))
            {
                using (TextWriter tw = File.AppendText(filename))
                {
                    tw.Write(content);
                }
            }
            else
            {
                using (TextWriter tw = File.CreateText(filename))
                {
                    tw.Write(content);
                }
            }
        }

        public static List<Cookie> ParseCookie(string cookiesStr, string url)
        {
            var cookies = new List<Cookie>();
            var temp = cookiesStr.Split(';');
            foreach (var cookieStr in temp)
            {
                var temp2 = cookieStr.Split('=');
                var name = temp2[0].Trim();
                var value = temp2[1];
                var path = "/";
                Uri uri = new Uri(url);
                var c = new Cookie(name, value, path, uri.Authority);
                cookies.Add(c);
            }
            return cookies;
        }
    }
}