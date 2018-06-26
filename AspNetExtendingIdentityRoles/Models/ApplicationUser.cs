using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;

namespace AspNetExtendingIdentityRoles.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        

        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> RececivedMessages { get; set; }
    }
}