using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated2.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rated.Web.Shared
{
    public class ProjectHelper
    {
        //public List<ProjectViewModel> GetProjectsPendingReviewerAcceptance()
        //{
        //    IProjectRepo projectRepo = new ProjectRepo();
        //    var userSession = new UserSession();
        //    var userId = userSession.GetUserSession().UserId;
        //    var projectsCore = projectRepo.GetProjectsPendingReviewerAcceptance(userId);
        //    var projectsView = BuildProjectView(projectsCore);

        //    return projectsView;
        //}

        public List<ProjectViewModel> GetProjectsForReviewerByStatus(Guid reviewerUserId, Enums.ProjectStatus projectStatusId)
        {
            IProjectRepo projectRepo = new ProjectRepo();
            var projectsCore = projectRepo.GetProjectsForReviewerByStatus(reviewerUserId, projectStatusId);
            var projectsView = BuildProjectView(projectsCore);
            return projectsView;
        }

        public List<ProjectViewModel> GetReviewerProjectsInProgress(Guid reviewerUserId)
        {
            IProjectRepo projectRepo = new ProjectRepo();
            var projectsCore = projectRepo.GetReviewerProjectsInProgress(reviewerUserId);
            var projectsView = BuildProjectView(projectsCore);
            return projectsView;
        }

        public List<ProjectViewModel> GetProjectsByStatus(Enums.ProjectStatus projectStatus)
        {
            var projectRepo = new ProjectRepo();
            var userSession = new UserSession();
            var userId = userSession.GetUserSession().UserId;
            var projectsCore = projectRepo.GetProjectsByStatus(userId, projectStatus);
            var projectsView = BuildProjectView(projectsCore);

            return projectsView;
        }

        public List<ProjectViewModel> BuildProjectView(List<ProjectCoreModel> projectsCore)
        {
            var projectsView = new List<ProjectViewModel>();

            foreach (var project in projectsCore)
            {
                projectsView.Add(new ProjectViewModel()
                {
                    ProjectDescription = project.ProjectDescription,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    CreatedDate = project.CreatedDate,
                    ModifiedDate = project.ModifiedDate,
                    ProjectDetailsCount = project.ProjectDetailsCount,
                    ProjectStatus = project.ProjectStatus,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                });
            }

            return projectsView;
        }

        public ProjectDetailViewModel BuildProjectDetailView(ProjectDetailCoreModel projectDetailCore)
        {
            return new ProjectDetailViewModel(){
                CreatedBy = projectDetailCore.CreatedBy,
                CreatedDate = projectDetailCore.CreatedDate,
                //DetailCount
                DetailDescription = projectDetailCore.ProjectDetailDescription,
                DetailName = projectDetailCore.ProjectDetailName,
                //HasReviewer
                HoursToComplete = projectDetailCore.HoursToComplete,
                ModifiedBy = projectDetailCore.ModifiedBy,
                ModifiedDate = projectDetailCore.ModifiedDate,
                ProjectDetailId = projectDetailCore.ProjectDetailId,
                ProjectId = projectDetailCore.ProjectId,
                ReviewerEmail = projectDetailCore.ReviewerEmail,
                ReviewerFirstName = projectDetailCore.ReviewerFirstName,
                //ReviewerFullName
                ReviewerLastName = projectDetailCore.ReviewerLastName,
                ReviewerStatus = (Enums.ProjectReviewerStatus)projectDetailCore.ReviewerStatusId,
                ReviewerStatusId = projectDetailCore.ReviewerStatusId,
                ReviewInstructions = projectDetailCore.ReviewInstructions,
                DetailStatus = (Enums.ProjectDetailStatus)projectDetailCore.StatusId,
                StatusId = projectDetailCore.StatusId,
            };
        }
    }
}