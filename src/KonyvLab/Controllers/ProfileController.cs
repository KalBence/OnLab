using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Logging;
using KonyvLab.dal.Managers;
using KonyvLab.Models.ProfileViewModels;
using KonyvLab.dal.Models;
using MongoDB.Bson;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KonyvLab.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        protected ReviewManager _reviewManager;
        protected MessageManager _messageManager = new MessageManager();
        protected NotificationManager _notificationManager = new NotificationManager();

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            ReviewManager rwManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _reviewManager = rwManager;
        }

      //  [HttpGet("{userName}")]
        [Route("Profile/Index/{userName}")]
        public async Task<IActionResult> Index(string userName)
        {
            ProfileViewModel pvm = new ProfileViewModel()
            {
                reviews = _reviewManager.FindByUserName(userName),
                User = await _userManager.FindByNameAsync(userName)
            };
            return View(pvm);
        }

        [HttpGet]
        [Route("Profile/SendMessage/{ToUserName}")]
        public IActionResult SendMessage(string ToUserName)
        {
            return View("SendMessage",ToUserName);
        }

        [HttpPost]
        public IActionResult SendMessage([FromForm] Message message)
        {
            _messageManager.AddNewMessage(message);
            return LocalRedirect("/");
        }

        [HttpGet]
        [Route("Profile/ViewMessages/{ToUserName}")]
        public IActionResult ViewMessages(string ToUserName)
        {
            return View(_messageManager.FindByUserName(ToUserName));
        }

        [HttpPost]
        public IActionResult Search(string SearchValue)
        {
            SearchViewModel searchViewModel = new SearchViewModel()
            {
                FoundUsers = _userManager.Users.Where(u => u.UserName.ToUpper().Contains(SearchValue.ToUpper())),
                FoundAuthor = _reviewManager.FindByAuthor(SearchValue),
                FoundTitle = _reviewManager.FindByTitle(SearchValue)
            };
            return View(searchViewModel);
        }

        [HttpGet]
        [Route("Profile/Follow/{userName}")]
        public async Task<IActionResult> Follow(string userName)
        {
            var LoggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            ObjectId LoggedInUserId = new ObjectId(LoggedInUser.Id);

            var User = _userManager.Users.Where(u => u.UserName == userName).SingleOrDefault();
            ObjectId UserId = new ObjectId(User.Id);

            if (!User.Subscribers.Contains(LoggedInUserId))
            {
                User.Subscribers.Add(LoggedInUserId);
                LoggedInUser.SubscribedTo.Add(UserId);

                IdentityResult result1 = await _userManager.UpdateAsync(LoggedInUser);
                IdentityResult result2 = await _userManager.UpdateAsync(User);
            }
            else
            {
                //error message
            }

            return LocalRedirect("/Profile/Index/" + userName);
        }

        [HttpGet]
        [Route("Profile/Unfollow/{userName}")]
        public async Task<IActionResult> Unfollow(string userName)
        {
            var LoggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            ObjectId LoggedInUserId = new ObjectId(LoggedInUser.Id);

            var User = _userManager.Users.Where(u => u.UserName == userName).SingleOrDefault();
            ObjectId UserId = new ObjectId(User.Id);

            if (User.Subscribers.Contains(LoggedInUserId))
            {
                User.Subscribers.Remove(LoggedInUserId);
                LoggedInUser.SubscribedTo.Remove(UserId);

                IdentityResult result1 = await _userManager.UpdateAsync(LoggedInUser);
                IdentityResult result2 = await _userManager.UpdateAsync(User);
            }
            else
            {
                //error message
            }

            return LocalRedirect("/Profile/Index/" + userName);
        }

        [HttpGet]
        [Route("Profile/ViewNotifications/{UserId}")]
        public async Task<IActionResult> ViewNotifications(string UserId)
        {
            var LoggedInUser = _userManager.GetUserAsync(HttpContext.User).Result;
            ObjectId LoggedInUserId = new ObjectId(LoggedInUser.Id);

            LoggedInUser.UnreadNotifications = 0;
            IdentityResult result = await _userManager.UpdateAsync(LoggedInUser);

            var not = _notificationManager.FindByUserId(UserId).ToList();
            ReadNotifications(UserId);

            return View(not);
        }

        public void ReadNotifications(string UserId)
        {
            _notificationManager.ReadNotifications(UserId);
        }

        [HttpGet]
        [Route("Profile/Following/{UserName}")]
        public IActionResult Following(string UserName)
        {
            var User = _userManager.Users.Where(u => u.UserName == UserName).SingleOrDefault();
            ObjectId UserId = new ObjectId(User.Id);

            List<ApplicationUser> Following = new List<ApplicationUser>();

            foreach(var u in User.SubscribedTo)
            {
                Following.Add(_userManager.Users.Where(q => q.Id == u.ToString()).SingleOrDefault());
            }

            return View(Following);
        }



    }
}
