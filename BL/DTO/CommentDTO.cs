using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class CommentDTO
    {
        public string ImageID { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public UserDTO User { get; set; }
    }
}
