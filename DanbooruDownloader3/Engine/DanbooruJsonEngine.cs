using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Engine
{
    public class DanbooruJsonEngine:IEngine 
    {
        public int? TotalPost { get; set; }
        public int? Offset { get; set; }
        public string RawData { get; set; }

        public string ResponseMessage { get; set; }
        public bool Success { get; set; }

        public System.ComponentModel.BindingList<Entity.DanbooruPost> Parse(string data, Entity.DanbooruSearchParam query)
        {
            throw new NotImplementedException();
        }

        public string GenerateQueryString(Entity.DanbooruSearchParam query)
        {
            throw new NotImplementedException();
        }


        public Entity.DanbooruSearchParam SearchParam
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int GetNextPage()
        {
            throw new NotImplementedException();
        }

        public int GetPrevPage()
        {
            throw new NotImplementedException();
        }
    }
}
