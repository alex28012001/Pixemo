using BL.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Images
{
    public class Image
    {
        public IEnumerable<ImageSize> Sizes { get; set; }


        public string WebformatURL { get; set; }
        public string FullHDImageURL { get; set; }
        public string PreviewURL { get; set; }
        public int WebformatHeight { get; set; }
        public int WebformatWidth { get; set; }
        public long ? OwnerId { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
