using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("ChatRooms")]
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<ClientProfile> Users{ get; set; }
    }
}
