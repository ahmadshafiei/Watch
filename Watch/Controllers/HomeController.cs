using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Watch.Business.Exceptions;
using Watch.DataAccess.Identity;
using Watch.Models;

namespace Watch.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager userManager;
        private SignInManager<User, int> signInManager;
        public IAuthenticationManager authenticationManager => HttpContext.GetOwinContext().Authentication;

        public HomeController(UserManager userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            signInManager = new SignInManager<User, int>(userManager, authenticationManager);
            User user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                ViewBag.Error = "کلمه ی عبور یا نام کاربری اشتباه است";
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(email, password, true, false);

            if (result == SignInStatus.Success && userManager.IsInRole(user.Id, "Admin"))
                return RedirectToAction("Index");
            ViewBag.Error = "کلمه ی عبور یا نام کاربری اشتباه است";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult LogOut()
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }
    }
}