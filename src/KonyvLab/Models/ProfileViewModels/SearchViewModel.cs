using KonyvLab.dal.Models;
using Microsoft.AspNetCore.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonyvLab.Models.ProfileViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<IdentityUser> FoundUsers { get; set; }
        public IEnumerable<Review> FoundTitle { get; set; }
        public IEnumerable<Review> FoundAuthor { get; set; }

    }
}
