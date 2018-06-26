using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspNetExtendingIdentityRoles.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public DateTime DateSent { get; set; }
        public bool DeletedBySender { get; set; }
        public bool DeletedByReceiver { get; set; }
        public string MessageBody { get; set; }
        public bool IsRead { get; set; }
        public string Location { get; set; }


        public string ReceiverID { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public string SenderID { get; set; }
        public virtual ApplicationUser Sender { get; set; }



    }

}