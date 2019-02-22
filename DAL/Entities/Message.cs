using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Message
    {
        [Key]
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public virtual ClientProfile User { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
    }
}

