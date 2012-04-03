using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DanbooruDownloader3.Entity
{
    [Serializable()]
    [System.Xml.Serialization.XmlInclude(typeof(System.Drawing.Bitmap))]
    public class DanbooruPost : INotifyPropertyChanged
    {
        public string Provider { get; set; }

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
        [Browsable(false)]
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
        [Browsable(false)]
        public string SampleUrl
        {
            get { return _sample_url; }
            set { this._sample_url = value; }
        }

        private int _sample_width;
        [Browsable(false)]
        public int SampleWidth
        {
            get { return _sample_width; }
            set { this._sample_width = value; }
        }

        private int _sample_height;
        [Browsable(false)]
        public int SampleHeight
        {
            get { return _sample_height; }
            set { this._sample_height = value; }
        }

        private string _status = "-";
        [Browsable(false)]
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
            set { thumbnailImage = value; }
        }

        [XmlIgnore]
        //[Browsable(false)]
        public List<DanbooruTag> TagsEntity { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [Browsable(false)]
        public int Filesize { get; set; }
    }
}
