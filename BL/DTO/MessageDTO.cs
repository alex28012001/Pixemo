using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class MessageDTO
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int ChatRoom_Id { get; set; }
        public UserDTO User { get; set; }
    }
}
