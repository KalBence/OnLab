using KonyvLab.dal.Managers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KonyvLab.Controllers
{
    public class HomeController : Controller
    {
        //ReviewManager reviewManager = new ReviewManager();
        ReviewManager _reviewManager;

        public HomeController(ReviewManager rwManager)
        {
            _reviewManager = rwManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_reviewManager.GetAllReviews());
        }
    }
}
