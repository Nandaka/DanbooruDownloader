using DanbooruDownloader3.Entity;
using System;
using System.ComponentModel;

namespace DanbooruDownloader3.Engine
{
    public interface IEngine
    {
        int? TotalPost { get; set; }
        int? Offset { get; set; }
        string RawData { get; set; }
        string ResponseMessage { get; set; }
        bool Success { get; set; }
        DanbooruSearchParam SearchParam { get; set; }

        BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam query, ref string errorMessage);

        String GenerateQueryString(DanbooruSearchParam query);

        int GetNextPage();

        int GetPrevPage();
    }
}