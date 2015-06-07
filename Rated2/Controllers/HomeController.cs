using Rated.Core.Infrastructure;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated2.Models;
using Rated2.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated2.Controllers
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

            var model = new UserProfileViewModel();
            model.Profile = new ProfileViewModel() {
                UserId = user.UserId,
                Score = 5, //user.Score,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Bio = user.Bio
            };

            model.Projects = new List<ProjectViewModel>() {
                new ProjectViewModel(){
                    Name = "PSC 5 Stars of Excellence",
                    Description = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur.",
                    Score = 2.2M,
                    ProjectId = Guid.NewGuid()
                },
                new ProjectViewModel(){
                    Name = "Medstar Windows Phone 8 Application",
                    Description = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur.",
                    Score = 3.2M,
                    ProjectId = Guid.NewGuid()
                }
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}