using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KonyvLab.dal.Models
{

    public class ApplicationUser : IdentityUser
    {
        public virtual List<ObjectId> Subscribers { get; set; }

        public virtual List<ObjectId> SubscribedTo { get; set; }

        public virtual int UnreadMessages { get; set; }

        public virtual int UnreadNotifications { get; set; }

    }
}
