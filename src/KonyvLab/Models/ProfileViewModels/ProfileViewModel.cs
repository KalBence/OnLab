using KonyvLab.dal.Models;
using System.Linq;

namespace KonyvLab.Models.ProfileViewModels
{
    public class ProfileViewModel
    {
        public IQueryable<Review> reviews { get; set; }
        public string userName { get; set; }

    }
}
