using System;
using System.ComponentModel;
using DanbooruDownloader3.Entity;
using System.Xml;

namespace DanbooruDownloader3.Engine
{
    public interface IEngine
    {
        int? TotalPost { get; set; }
        int? Offset { get; set; }
        string RawData { get; set; }

        BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam query);

        String GenerateQueryString(DanbooruSearchParam query);
        
    }
}
