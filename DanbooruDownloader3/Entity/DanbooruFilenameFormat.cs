using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Entity
{
    public class DanbooruFilenameFormat
    {
        private string _FilenameFormat;

        public string FilenameFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FilenameFormat))
                {
                    _FilenameFormat = @"%provider%\%rating% - %id% - %tags%";
                }
                return _FilenameFormat;
            }
            set
            {
                _FilenameFormat = value;
            }
        }

        private int _Limit;

        public int Limit
        {
            get
            {
                if (_Limit <= 0 || _Limit > FormMain.MAX_FILENAME_LENGTH)
                {
                    _Limit = 150;
                }
                return _Limit;
            }
            set
            {
                _Limit = value;
            }
        }

        private string _BaseFolder;

        public string BaseFolder
        {
            get
            {
                if (_BaseFolder == null)
                {
                    _BaseFolder = "";
                }
                return _BaseFolder;
            }
            set
            {
                _BaseFolder = value;
            }
        }

        private string _MissingTagReplacement;

        public string MissingTagReplacement
        {
            get
            {
                if (_MissingTagReplacement == null)
                {
                    _MissingTagReplacement = "";
                }
                return _MissingTagReplacement;
            }
            set
            {
                _MissingTagReplacement = value;
            }
        }

        private string _artistGroupReplacement;
        private string _copyrightGroupReplacement;
        private string _characterGroupReplacement;
        private string _circleGroupReplacement;
        private string _faultsGroupReplacement;

        public int ArtistGroupLimit { get; set; }

        public string ArtistGroupReplacement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_artistGroupReplacement))
                {
                    _artistGroupReplacement = "multiple_artists";
                }
                return _artistGroupReplacement;
            }
            set
            {
                _artistGroupReplacement = value;
            }
        }

        public int CopyrightGroupLimit { get; set; }

        public string CopyrightGroupReplacement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_copyrightGroupReplacement))
                {
                    _copyrightGroupReplacement = "multiple_copyrights";
                }
                return _copyrightGroupReplacement;
            }
            set
            {
                _copyrightGroupReplacement = value;
            }
        }

        public int CharacterGroupLimit { get; set; }

        public string CharacterGroupReplacement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_characterGroupReplacement))
                {
                    _characterGroupReplacement = "multiple_characters";
                }
                return _characterGroupReplacement;
            }
            set
            {
                _characterGroupReplacement = value;
            }
        }

        public int CircleGroupLimit { get; set; }

        public string CircleGroupReplacement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_circleGroupReplacement))
                {
                    _circleGroupReplacement = "multiple_circles";
                }
                return _circleGroupReplacement;
            }
            set
            {
                _circleGroupReplacement = value;
            }
        }

        public int FaultsGroupLimit { get; set; }

        public string FaultsGroupReplacement
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_faultsGroupReplacement))
                {
                    _faultsGroupReplacement = "multiple_faults";
                }
                return _faultsGroupReplacement;
            }
            set
            {
                _faultsGroupReplacement = value;
            }
        }

        public List<DanbooruTag> IgnoredTags { get; set; }

        public String IgnoredTagsRegex { get; set; }

        public bool IgnoreTagsUseRegex { get; set; }

        public bool IsReplaceMode { get; set; }
    }
}