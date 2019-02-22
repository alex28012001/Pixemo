using BL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Images
{
    public class ImageResult : BaseResult
    {
        public IEnumerable<Image> Images { get; set; }
        public int ? CurrentNumberGroups { get; set; }
    }     
}
