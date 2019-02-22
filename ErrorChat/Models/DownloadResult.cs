using System.Collections.Generic;

namespace ErrorChat.Models
{
    public class DownloadResult
    {
        public IEnumerable<string> ImagesUrl { get; set; }
        public string AdLink { get; set; }
        public string ImageId { get; set; }
        public bool Successed { get; set; }
        public string ErrorMessage { get; set; }
      
    }
}