﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Entities.Comments
{
    public class Comment
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public string Text { get; set; }
        public DateTime ? Date { get; set; }
    }
}
