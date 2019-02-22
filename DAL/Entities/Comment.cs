using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public long CommentID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string ImageID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
