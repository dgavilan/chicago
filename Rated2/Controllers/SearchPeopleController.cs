using Rated.Core.Contracts;
using Rated.Infrastructure.Database.Repository;
using Rated.Models.UserProfile;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated.Web.Controllers
{
    public class SearchPeopleController : Controller
    {
        // GET: SearchPeople
        public ActionResult Index()
        {
            IUserRepo userRepo = new UserRepo();
            var users = userRepo.GetUsers();

            // Map from Core to ViewModel
            var profiles = new List<ProfileViewModel>();
            foreach (var user in users)
            {
                profiles.Add(new ProfileViewModel() { 
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserId = user.UserId,
                });
            }

            return View(profiles);
        }

        [HttpGet]
        public ActionResult ProfileView(Guid userId)
        {
            var projectRepo = new ProjectRepo();

            var projects = projectRepo.GetProjectsByUserId(userId);

            var projectHelper = new ProjectHelper();

            return View("Profile", projectHelper.BuildProjectView(projects));
        }
    
    }
}