using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DanbooruDownloader3
{
    public static class Constants
    {

        public static Dictionary<string, string> OrderBy = new Dictionary<string, string>() 
                                                            {
                                                                {"None", null},
                                                                {"Score", "order:score"},
                                                                {"Score (Ascending)", "order:score_asc"},
                                                                {"Score (Descending)", "order:score_desc"},
                                                                {"Favorite Count", "order:favcount"},
                                                                {"MegaPixel", "order:mpixels"},
                                                                {"MegaPixel Ascending", "order:mpixels_asc"},
                                                                {"Megapixels (Descending)", "order:mpixels_desc"},
                                                                {"Count", "order:Count"},
                                                                {"Name", "order:name"},
                                                                {"Date", "order:date"},
                                                                {"Id", "order:id"},
                                                                {"Id (Ascending)", "order:id_asc"},
                                                                {"Id (Descending)", "order:id_desc"},
                                                                {"Landscape", "order:landscape"},
                                                                {"Portrait", "order:portrait"},
                                                                {"Popularity", "order:popular"},
                                                                {"Filesize (Descending)", "order:filesize"}
                                                            };

        public static Dictionary<string, string> Rating = new Dictionary<string, string>() 
                                                        {
                                                            {"All", null},
                                                            {"Safe", "rating:safe"},
                                                            {"Questionable", "rating:questionable"},
                                                            {"Explicit", "rating:explicit"}
                                                        };
        public static string LOADING_URL = "Loading...";
        public static string NO_POST_PARSER = "No parser available...";
    }
}
