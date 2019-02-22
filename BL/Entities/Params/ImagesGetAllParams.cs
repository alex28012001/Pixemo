using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Params
{
    public class ImagesGetAllParams:Params
    {
        public int ? Offset { get; set; }

        public ImagesGetAllParams(int ownerId, string query = null, int count = 20,int ? offset = null) 
            :base(query,ownerId,count)
        {
            Offset = offset;
        }
    }
}
