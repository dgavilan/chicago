using Rated.Core.Contracts;
using Rated.Core.Infrastructure;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated.Models;
using Rated.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
         {
            var userSession = new UserSession();
            if (!userSession.IsLoggedIn())
            { 
                // NOTE: User is not looged in. Redirect to login page.
                return Redirect("~/userprofile/login");
            }

            var user = userSession.GetUserSession();

            //var profileRepo = new ProfileRepo();
            //var userId = Guid.NewGuid();
            //var profile = profileRepo.GetUserProfileByUserId(userId);

            IProjectRepo projectRepo = new ProjectRepo();
            var userRating = projectRepo.GetUserRating(user.UserId);

            var model = new UserProfileViewModel();
            model.Profile = new ProfileViewModel() {
                UserId = user.UserId,
                Score = Convert.ToDouble(Math.Round(userRating, 2)),
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Bio = user.Bio
            };

            return View(model);
        }

        
    }
}