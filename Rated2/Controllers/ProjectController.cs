using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Project
        //public ActionResult Index(Enums.ProjectStatus projectType)
        //{
        //    var projectRepo = new ProjectRepo();
        //    var userSession = new UserSession();
        //    var userId = userSession.GetUserSession().UserId;

        //    var projectsCore = projectRepo.GetProjectsByStatus(userId, projectType);

        //    var projectsView = new List<ProjectViewModel>();

        //    foreach (var project in projectsCore)
        //    {
        //        projectsView.Add(new ProjectViewModel()
        //        {
        //            ProjectDescription = project.ProjectDescription,
        //            ProjectId = project.ProjectId,
        //            ProjectName = project.ProjectName,
        //            CreatedDate = project.CreatedDate,
        //            ProjectDetailsCount = project.ProjectDetailsCount,
        //            ProjectStatus = project.ProjectStatus,
        //            OwnerFirstName = project.OwnerFirstName,
        //            OwnerLastName = project.OwnerLastName,

        //        });
        //    }

        //    return View(projectsView);
        //}

        public ActionResult Draft()
        {
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.Draft));
        }

        public ActionResult Pending()
        {
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.ReviewerPendingAcceptance));
        }

        public ActionResult InProgress()
        {
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.InProgress));
        }

        public ActionResult Done()
        {
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.Done));
        }

        [HttpGet]
        public ActionResult StartProject()
        {
            var projectView = new ProjectViewModel()
            {
                Tasks = new List<TaskViewModel>()
            };

            ViewBag.OpenAddProjectModal = 1;
            return View(projectView);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var userSession = new UserSession();
            var user = userSession.GetUserSession();
            var projectRepo = new ProjectRepo();
            var projectCore = projectRepo.GetProjectByProjectId(id);
            var projectDetailsCore = projectRepo.GetProjectDetailsByProjectId(id);
            var projectView = MapToProjectView(projectCore, projectDetailsCore);

            ViewBag.OpenAddProjectModal = 0;

            return View("Edit", projectView);
        }

        //[HttpGet]
        //public ActionResult ViewReview(Guid id)
        //{
        //    var userSession = new UserSession();
        //    var user = userSession.GetUserSession();
        //    var projectRepo = new ProjectRepo();
        //    var projectCore = projectRepo.GetProjectByProjectId(id);
        //    var projectDetailsCore = projectRepo.GetProjectDetailByProjectId(id);
        //    var projectView = MapToProjectView(projectCore, projectDetailsCore);

        //    return View("Edit", projectView);
        //}

        private object MapToProjectView(ProjectCoreModel projectCore, List<TaskCoreModel> projectDetailsCore)
        {
            var detailView = new List<TaskViewModel>();

            foreach (var detail in projectDetailsCore)
            {
                detailView.Add(new TaskViewModel()
                {
                    CreatedBy = detail.CreatedBy,
                    CreatedDate = detail.CreatedDate,
                    TaskCount = 0,
                    Description = detail.Description,
                    Name = detail.Name,
                    ModifiedBy = detail.ModifiedBy,
                    ModifiedDate = detail.ModifiedDate,
                    TaskId = detail.TaskId,
                    ProjectId = detail.ProjectId,
                    HoursToComplete = detail.HoursToComplete,
                    ReviewerFirstName = detail.ReviewerFirstName,
                    ReviewerLastName = detail.ReviewerLastName,
                    ReviewerEmail = detail.ReviewerEmail,
                    ReviewerStatusId = detail.ReviewerStatusId,
                    //ReviewerStatus = (Enums.ProjectReviewerStatus)detail.ReviewerStatusId,
                    //ReviewerFullName = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.WaitingForReviewerToAccept)
                    //    ? detail.ReviewerEmail
                    //    : detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    ReviewerFullName = detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    HasReviewer = detail.HasReviewer,
                    StatusId = detail.StatusId,
                    DetailStatus = detail.Status,
                    ReviewInstructions = detail.ReviewInstructions,
                    Rating = detail.Rating,
                });
            }

            var projectView = new ProjectViewModel()
            {
                CreatedBy = projectCore.CreatedBy,
                CreatedDate = projectCore.CreatedDate,
                ModifiedBy = projectCore.ModifiedBy,
                ModifiedDate = projectCore.ModifiedDate,
                ProjectDescription = projectCore.ProjectDescription,
                ProjectDetailsCount = projectCore.ProjectDetailsCount,
                ProjectId = projectCore.ProjectId,
                Tasks = detailView,
                ProjectName = projectCore.ProjectName,
                ProjectStatus = projectCore.ProjectStatus,
                ReviewerEmail = ""
            };

            return projectView;
        }

        

    }
}
