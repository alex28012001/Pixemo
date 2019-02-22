using BL.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Base
{
    public abstract class BaseResult
    {
        public long TotalCount { get; set; }
    }
}
