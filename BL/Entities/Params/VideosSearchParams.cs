using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Params
{
    public class VideosSearchParams:Params
    {
        public int ? Offset { get; set; }
        public VideosSearchParams(string query,int count,int ? offset,long id = 0)
            :base(query,id,count)
        {
            Offset = offset;
        }
    }
}
