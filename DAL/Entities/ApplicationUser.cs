using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ClientProfile ClientProfile { get; set; } 
    }
}
