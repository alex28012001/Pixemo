using BL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Videos
{
    public class VideoResult:BaseResult
    {
        public IEnumerable<Video> Videos { get; set; }
    }
}
