using DanbooruDownloader3.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DanbooruDownloader3.Entity
{
    [Serializable()]
    [System.Xml.Serialization.XmlInclude(typeof(System.Drawing.Bitmap))]
    public class DanbooruPost : INotifyPropertyChanged
    {
        public DanbooruProvider Provider { get; set; }

        public string Query { get; set; }

        public string SearchTags { get; set; }

        public string Referer { get; set; }

        [Browsable(false)]
        public bool Completed { get; set; }

        private string _id;

        public string Id
        {
            get { return _id; }
            set { this._id = value; }
        }

        private string _tags;

        public string Tags
        {
            get { return _tags; }
            set { this._tags = value; }
        }

        private string _source;

        public string Source
        {
            get { return _source; }
            set { this._source = value; }
        }

        private string _score;

        public string Score
        {
            get { return _score; }
            set { this._score = value; }
        }

        private string _rating;

        public string Rating
        {
            get { return _rating; }
            set { this._rating = value; }
        }

        private string _status = "-";

        public string Status
        {
            get { return _status; }
            set { this._status = value; }
        }

        private bool _has_children;

        [Browsable(false)]
        public bool HasChildren
        {
            get { return _has_children; }
            set { this._has_children = value; }
        }

        private string _parent_id;

        [Browsable(false)]
        public string ParentId
        {
            get { return _parent_id; }
            set { this._parent_id = value; }
        }

        private string _change;

        [Browsable(false)]
        public string Change
        {
            get { return _change; }
            set { this._change = value; }
        }

        private string _creator_id;

        [Browsable(false)]
        public string CreatorId
        {
            get { return _creator_id; }
            set { this._creator_id = value; }
        }

        private string _created_at;

        [Browsable(false)]
        public string CreatedAt
        {
            get { return _created_at; }
            set { this._created_at = value; }
        }

        private string _md5;

        public string MD5
        {
            get { return _md5; }
            set { this._md5 = value; }
        }

        [XmlIgnore]
        private Image thumbnailImage;

        [Browsable(true)]
        [XmlIgnore]
        public Image ThumbnailImage
        {
            get { return thumbnailImage; }
            set
            {
                thumbnailImage = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("ThumbnailImage"));
            }
        }

        private List<DanbooruTag> tagsEntity;

        [XmlIgnore]
        //[Browsable(false)]
        public List<DanbooruTag> TagsEntity
        {
            get
            {
                if (tagsEntity == null)
                {
                    tagsEntity = Helper.ParseTags(Tags, Provider);
                }
                return tagsEntity;
            }
            set { this.tagsEntity = value; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        #endregion INotifyPropertyChanged Members

        public int Filesize { get; set; }

        public string Filename { get; set; }

        [Browsable(false)]
        public bool Hidden { get; set; }

        #region image/thumbnail url related

        /* if jpeg_url not exist, then the url will be taken from sample_url
         * if sample_url not exist, then the url will be taken from file_url
         */
        private string _file_url;

        public string FileUrl
        {
            get { return _file_url; }
            set { this._file_url = value; }
        }

        private int _width;

        public int Width
        {
            get { return _width; }
            set { this._width = value; }
        }

        private int _height;

        public int Height
        {
            get { return _height; }
            set { this._height = value; }
        }

        private string _preview_url;

        public string PreviewUrl
        {
            get { return _preview_url; }
            set { this._preview_url = value; }
        }

        private int _preview_width;

        [Browsable(false)]
        public int PreviewWidth
        {
            get { return _preview_width; }
            set { this._preview_width = value; }
        }

        private int _preview_height;

        [Browsable(false)]
        public int PreviewHeight
        {
            get { return _preview_height; }
            set { this._preview_height = value; }
        }

        private string _sample_url;

        public string SampleUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_sample_url))
                {
                    _sample_url = FileUrl;
                }
                return _sample_url;
            }
            set { this._sample_url = value; }
        }

        private int _sample_width;

        [Browsable(false)]
        public int SampleWidth
        {
            get
            {
                if (_sample_width == 0)
                {
                    _sample_width = Width;
                }
                return _sample_width;
            }
            set { this._sample_width = value; }
        }

        private int _sample_height;

        [Browsable(false)]
        public int SampleHeight
        {
            get
            {
                if (_sample_height == 0)
                {
                    _sample_height = Height;
                }
                return _sample_height;
            }
            set { this._sample_height = value; }
        }

        private string _jpeg_url;

        public string JpegUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_jpeg_url))
                {
                    _jpeg_url = SampleUrl;
                }
                return _jpeg_url;
            }
            set { this._jpeg_url = value; }
        }

        private int _jpeg_width;

        [Browsable(false)]
        public int JpegWidth
        {
            get
            {
                if (_jpeg_width == 0)
                {
                    _jpeg_width = Width;
                }
                return _jpeg_width;
            }
            set { this._jpeg_width = value; }
        }

        private int _jpeg_height;

        [Browsable(false)]
        public int JpegHeight
        {
            get
            {
                if (_jpeg_height == 0)
                {
                    _jpeg_height = Height;
                }
                return _jpeg_height;
            }
            set { this._jpeg_height = value; }
        }

        #endregion image/thumbnail url related
    }
}