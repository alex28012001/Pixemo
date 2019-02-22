using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class AdvertisingDTO
    {
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string AdvertisingLink { get; set; }
        public string OwnerId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
