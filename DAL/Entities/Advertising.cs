using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

    public class Advertising
    {
        [Key]
        public int Id { get; set; }
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string AdvertisingLink { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
