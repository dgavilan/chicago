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

namespace Rated.Web.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pending()
        {
            //var projectHelper = new ProjectHelper();
            //return View(projectHelper.GetProjectsPendingReviewerAcceptance());
            var userSession = new UserSession();
            var reviewerUserId = userSession.GetUserSession().UserId;
            var projectHelper = new ProjectHelper();
            return View(projectHelper.GetProjectsForReviewerByStatus(reviewerUserId, Enums.ProjectStatus.ReviewerPendingAcceptance));
        }

        public ActionResult InProgress()
        {
            var userSession = new UserSession();
            var reviewerUserId = userSession.GetUserSession().UserId;
            var projectHelper = new ProjectHelper();

            ViewBag.ReviewerProjectStatus = Enums.ReviewerProjectStatus.InProgress;

            return View(projectHelper.GetReviewerProjectsInProgress(reviewerUserId));
        }

        public ActionResult Done()
        {
            var userSession = new UserSession();
            var reviewerUserId = userSession.GetUserSession().UserId;
            var projectHelper = new ProjectHelper();

            ViewBag.ReviewerProjectStatus = Enums.ReviewerProjectStatus.Done;

            return View(projectHelper.GetReviewerProjectsDone(reviewerUserId));
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var userSession = new UserSession();
            var user = userSession.GetUserSession();
            var projectRepo = new ProjectRepo();
            var projectCore = projectRepo.GetProjectForReviewerByProjectIdByUserId(id, user.UserId);
            var projectDetailsCore = projectRepo.GetProjectDetailsByProjectIdByUserId(id, user.UserId);
            var projectView = MapToProjectView(projectCore, projectDetailsCore);

            return View("Edit", projectView);
        }

        private ProjectViewModel MapToProjectView(ProjectCoreModel projectCore, List<TaskCoreModel> projectDetailsCore)
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
                    //ReviewerFullName = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.WaitingForReviewerToAccept)
                    //    ? detail.ReviewerEmail
                    //    : detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    ReviewerFullName = detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    HasReviewer = detail.HasReviewer,
                    DetailStatus = (Enums.TaskStatus)detail.StatusId,
                    StatusId = detail.StatusId,
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
                ReviewerEmail = "",
                OwnerFirstName = projectCore.OwnerFirstName,
                OwnerLastName = projectCore.OwnerLastName,
                ReviewerProjectStatus = projectCore.ReviewerProjectStatus,
            };

            return projectView;
        }

    }
}