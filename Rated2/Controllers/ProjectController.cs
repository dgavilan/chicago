using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated2.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated2.Controllers
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
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.WaitingApproverAcceptance));
        }

        public ActionResult InProgress()
        {
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsByStatus(Enums.ProjectStatus.InProgressWaitingForOwnerToFinishProject));
        }

        public ActionResult Completed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StartProject()
        {
            var projectView = new ProjectViewModel()
            {
                ProjectDetails = new List<ProjectDetailViewModel>()
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
            var projectCore = projectRepo.GetProject(user.UserId, id);
            var projectDetailsCore = projectRepo.GetProjectDetail(user.UserId, id);
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

        private object MapToProjectView(ProjectCoreModel projectCore, List<ProjectDetailCoreModel> projectDetailsCore)
        {
            var detailView = new List<ProjectDetailViewModel>();

            foreach (var detail in projectDetailsCore)
            {
                detailView.Add(new ProjectDetailViewModel()
                {
                    CreatedBy = detail.CreatedBy,
                    CreatedDate = detail.CreatedDate,
                    DetailCount = 0,
                    DetailDescription = detail.ProjectDetailDescription,
                    DetailName = detail.ProjectDetailName,
                    ModifiedBy = detail.ModifiedBy,
                    ModifiedDate = detail.ModifiedDate,
                    ProjectDetailId = detail.ProjectDetailId,
                    ProjectId = detail.ProjectId,
                    HoursToComplete = detail.HoursToComplete,
                    ReviewerFirstName = detail.ReviewerFirstName,
                    ReviewerLastName = detail.ReviewerLastName,
                    ReviewerEmail = detail.ReviewerEmail,
                    ReviewerStatusId = detail.ReviewerStatusId,
                    ReviewerStatus = (Enums.ProjectReviewerStatus)detail.ReviewerStatusId,
                    ReviewerFullName = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.Sent)
                        ? detail.ReviewerEmail
                        : detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    HasReviewer = detail.HasReviewer,
                    StatusId = detail.StatusId,
                    Status = (Enums.ProjectDetailStatus)detail.StatusId,

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
                ProjectDetails = detailView,
                ProjectName = projectCore.ProjectName,
                ProjectStatus = projectCore.ProjectStatus,
                ReviewerEmail = ""
            };

            return projectView;
        }

    }
}
