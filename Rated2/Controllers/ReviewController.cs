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
            return View(projectHelper.GetReviewerProjectsInProgress(reviewerUserId));
        }

        public ActionResult Completed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var userSession = new UserSession();
            var user = userSession.GetUserSession();
            var projectRepo = new ProjectRepo();
            var projectCore = projectRepo.GetProjectByProjectId(id);
            var projectDetailsCore = projectRepo.GetProjectDetailsByProjectId(id, user.UserId);
            var projectView = MapToProjectView(projectCore, projectDetailsCore);

            return View("Edit", projectView);
        }

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
                    ReviewerFullName = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.WaitingForReviewerToAccept)
                        ? detail.ReviewerEmail
                        : detail.ReviewerFirstName + " " + detail.ReviewerLastName,
                    HasReviewer = detail.HasReviewer,
                    DetailStatus = (Enums.ProjectDetailStatus)detail.StatusId,
                    StatusId = detail.StatusId,
                    ReviewInstructions = detail.ReviewInstructions,
                    DetailRating = detail.DetailRating,
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
                ReviewerEmail = "",
                OwnerFirstName = projectCore.OwnerFirstName,
                OwnerLastName = projectCore.OwnerLastName,
            };

            return projectView;
        }

    }
}