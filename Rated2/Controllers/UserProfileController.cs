using Rated.Core.Models.User;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated.Controllers
{
    public class UserProfileController : Controller
    {
        UserCoreService _userService;

        public UserProfileController()
        {
            _userService = new UserCoreService(new UserRepo());
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string userPassword)
        {
            var userCore = _userService.Login(email, userPassword);
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
        public ActionResult CreateAccount(string email, string userPassword, string firstName, string lastName, string bio)
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
                    Password = userPassword,
                    Bio = bio,
                    LastLoginDate = Convert.ToDateTime("1/1/1990"),
                    StatusId = Enums.UserStatus.ReviewerAccepted,
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

        [HttpPost]
        public ActionResult EditAccount(string email, string firstName, string lastName, string bio)
        {
            _userService.EditAccount(new UserCoreModel() { 
                Bio = bio,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ModifiedDate = DateTime.UtcNow
            });
            return View();
        }

        public ActionResult EditAccount()
        {
            var userSession = new UserSession();
            var user = userSession.GetUserSession();
            var userView = new ProfileViewModel() {
                Bio = user.Bio,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.UserId,
                UserPassword = ""
            };

            return View(userView);
        }

        
    }
}