using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Params
{
    public class GroupSearchParams:Params
    {
        public int ? Offset { get; set; }

        public GroupSearchParams(string query, int id = 0, int count = 20, int ? offset = null)
            : base(query, id, count) 
        {
            Offset = offset;
        }
    }
}
