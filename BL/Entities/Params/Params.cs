using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Params
{
    public abstract class Params
    {
        public long Id { get; set; }
        public string Query { get; set; }
        public int Count { get; set; }


        protected Params(string query,long id,int count)
        {
            Query = query;
            Id = id;
            Count = count;
        }
    }
}
