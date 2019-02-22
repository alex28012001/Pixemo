using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Videos
{
    public class Video
    {
        public Uri SmallPoster { get; set; }
        public Uri BigPoster { get; set; }
        public Uri MediumPoster { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Uri Url { get; set; } 
        public string Title { get; set; }
    }
}
