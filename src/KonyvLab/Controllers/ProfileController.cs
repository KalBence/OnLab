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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KonyvLab.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        protected ReviewManager _reviewManager;
        protected MessageManager _messageManager = new MessageManager();
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
        public IActionResult Index(string userName)
        {
            ProfileViewModel pvm = new ProfileViewModel();
            pvm.reviews = _reviewManager.FindByUserName(userName);
            pvm.userName = userName;
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
            SearchViewModel searchViewModel = new SearchViewModel();
            //beleírjak az alap UserManager implementációba?
            searchViewModel.FoundUsers = _userManager.Users.Where(u => u.UserName.ToUpper().Contains(SearchValue.ToUpper()));
            searchViewModel.FoundAuthor = _reviewManager.FindByAuthor(SearchValue);
            searchViewModel.FoundTitle = _reviewManager.FindByTitle(SearchValue);

            return View(searchViewModel);
        }

    }
}
