using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Params
{
    public class ImageSearchParams: Params
    {
        public int ? Offset { get; set; }
        public int? OffsetGroups { get; set; }

        public ImageSearchParams(string query,long id = 0,int count = 20,int ? offset = null, int? offsetGroups = null)
            :base(query,id,count)
        {
            Offset = offset;
            OffsetGroups = offsetGroups;
        }
    }
}
