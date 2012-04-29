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

    }
}
