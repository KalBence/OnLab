using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace test.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index()
        {
            return View("Error");
        }

        public IActionResult Error(string Id)
        {
            return View("Error404");
        }
    }
}
