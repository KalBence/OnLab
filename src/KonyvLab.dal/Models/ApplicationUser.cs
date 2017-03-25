using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace KonyvLab.dal.Models
{

    public class ApplicationUser : IdentityUser
    {
        public List<IdentityUser> Subscibers { get; set; }

        public List<IdentityUser> SubscribedTo { get; set; }
    }
}
