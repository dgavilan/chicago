using Rated.Core.Models.User;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated2.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string userPassword)
        {
            var userSessionCore = new UserCoreService(new UserRepo());
            var userCore = userSessionCore.Login(email, userPassword);
            if (userCore.UserId != Guid.Empty)
            {
                var userSession = new UserSession();
                userSession.InitializeUserSession(userCore);
                return Redirect("~/home/index");
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var userSession = new UserSession();
            userSession.ClearUserSession();

            return Redirect("~/userprofile/login");
        }

        [HttpPost]
        public ActionResult CreateAccount(string email, string userPassword, string firstName, string lastName)
        {
            if (ModelState.IsValid)
            {
                var userRepo = new UserRepo();
                var timeStamp = DateTime.UtcNow;

                userRepo.CreateUser(new UserCoreModel()
                {
                    CreatedDate = timeStamp,
                    Email = email,
                    FirstName = firstName,
                    IsActive = true,
                    LastName = lastName,
                    ModifiedDate = timeStamp,
                    UserId = Guid.NewGuid(),
                    Password = userPassword
                });

                ViewBag.UserCreated = true;
            }
            else
            {
                ViewBag.UserCreated = false;                
            }

            return View();
        }

        public ActionResult CreateAccount()
        {
            ViewBag.UserCreated = false;
            return View();
        }
    }
}