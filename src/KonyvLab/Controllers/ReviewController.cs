using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Logging;
using KonyvLab.dal.Managers;
using KonyvLab.dal.Models;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KonyvLab.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        protected ReviewManager _reviewManager;
        protected NotificationManager _notificationManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public ReviewController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILoggerFactory loggerFactory,
        ReviewManager rwManager,
        NotificationManager notificationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ReviewController>();
            _reviewManager = rwManager;
            _notificationManager = notificationManager;
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
        public async Task<IActionResult> Add(Review review)
        {
            var LoggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            review.UserName = LoggedInUser.UserName;
            _reviewManager.AddNewReview(review);
            
            foreach(var u in LoggedInUser.Subscribers)
            {
                Notification n = new Notification()
                {
                    UserId = u,
                    Object = review._id,
                    Time = DateTime.Now,
                    Message = LoggedInUser + " has just uploaded a new Review. ",
                    WasRead = false
                };
                _notificationManager.AddNewNotification(n);
                var TargetUser = _userManager.FindByIdAsync(u.ToString()).Result;
                TargetUser.UnreadNotifications++;
                IdentityResult result = await _userManager.UpdateAsync(TargetUser);

            }
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
            _reviewManager.IncreaseViewCount(Id);
            return View(_reviewManager.FindById(Id));
        }

        [HttpGet]
        public IActionResult Update(string Id)
        {
            return View("Update", _reviewManager.FindById(Id));
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
