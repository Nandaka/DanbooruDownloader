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
                                                                {"ScoreAscending", "order:score_asc"},
                                                                {"FavoriteCount", "order:favcount"},
                                                                {"MegaPixel", "order:mpixels"},
                                                                {"MegaPixelAscending", "order:mpixels_asc"},
                                                                {"Count", "order:count"},
                                                                {"Name", "order:name"},
                                                                {"Date", "order:date"},
                                                                {"Id", "order:id"},
                                                                {"IdAscending", "order:id_asc"},
                                                                {"Landscape", "order:landscape"},
                                                                {"Portrait", "order:portrait"}        
                                                            };

        public static Dictionary<string, string> Rating = new Dictionary<string, string>() 
                                                        {
                                                            {"All", null},
                                                            {"Safe", "rating:safe"},
                                                            {"Questionable", "rating:questionable"},
                                                            {"Explicit", "rating:explicit"}
                                                        };
    }
}
