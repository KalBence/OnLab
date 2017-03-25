using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Logging;
using KonyvLab.dal.Managers;
using KonyvLab.dal.Models;
using MongoDB.Bson;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KonyvLab.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        protected ReviewManager _reviewManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        public ReviewController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILoggerFactory loggerFactory,
        ReviewManager rwManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ReviewController>();
            _reviewManager = rwManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("AddForm");
        }

        [HttpPost]
        public IActionResult Add(Review review)
        {
            review.UserName = _userManager.GetUserName(HttpContext.User);
            _reviewManager.AddNewReview(review);
            return LocalRedirect("/");
        }

        [HttpGet]
        public IActionResult AddDetails()
        {
            return View("AddDetails");
        }

        [HttpGet]
        public IActionResult Read(string Id)
        {
            Review review = _reviewManager.FindById(Id);
            _reviewManager.IncreaseViewCount(Id);
            return View(review);
        }

        [HttpGet]
        public IActionResult Update(string Id)
        {
            Review review = _reviewManager.FindById(Id);
            return View("Update", review);
        }

        [HttpPost]
        public IActionResult Update([FromRoute]string id, Review review)
        {
            ObjectId oid = new ObjectId(id);
            review._id = oid;
            _reviewManager.Update(review);
            return LocalRedirect("/Profile/Index/"+ _userManager.GetUserName(User));
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            _reviewManager.Delete(id);
            return LocalRedirect("/Profile/Index/" + _userManager.GetUserName(User));
        }


    }
}
