using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Models.Project;
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
            var projectsView = BuildProjectsView(projectsCore);
            return projectsView;
        }

        public List<ProjectViewModel> GetReviewerProjectsInProgress(Guid reviewerUserId)
        {
            IProjectRepo projectRepo = new ProjectRepo();
            var projectsCore = projectRepo.GetReviewerProjectsInProgress(reviewerUserId);
            var projectsView = BuildProjectsView(projectsCore);
            return projectsView;
        }

        public List<ProjectViewModel> GetProjectsByStatus(Enums.ProjectStatus projectStatus)
        {
            var projectRepo = new ProjectRepo();
            var userSession = new UserSession();
            var userId = userSession.GetUserSession().UserId;
            var projectsCore = projectRepo.GetProjectsByStatus(userId, projectStatus);
            var projectsView = BuildProjectsView(projectsCore);

            return projectsView;
        }

        public List<ProjectViewModel> BuildProjectsView(List<ProjectCoreModel> projectsCore)
        {
            var projectsView = new List<ProjectViewModel>();

            foreach (var project in projectsCore)
            {
                projectsView.Add(BuildProjectView(project));
            }

            return projectsView;
        }

        public ProjectViewModel BuildProjectView(ProjectCoreModel project)
        {
            return new ProjectViewModel()
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
                    ProjectRating = project.ProjectRating,
                };
        }

        //public ProjectDetailViewModel BuildProjectDetailView(ProjectDetailCoreModel detail)
        //{
        //    return new ProjectDetailViewModel()
        //    {
        //        CreatedBy = detail.CreatedBy,
        //        CreatedDate = detail.CreatedDate,
        //        DetailCount = 0,
        //        DetailDescription = detail.ProjectDetailDescription,
        //        DetailName = detail.ProjectDetailName,
        //        DetailRating = detail.DetailRating,
        //        DetailStatus = detail.DetailStatus,
        //        HasReviewer = detail.HasReviewer,
        //        HoursToComplete = detail.HoursToComplete,
        //        ModifiedBy = detail.ModifiedBy,
        //        ModifiedDate = detail.ModifiedDate,
        //        ProjectDetailId = detail.ProjectDetailId,
        //        ProjectId = detail.ProjectId,
        //        ReviewerEmail = detail.ReviewerEmail,
        //        ReviewerFirstName = detail.ReviewerFirstName,
        //        ReviewerFullName = "",
        //        ReviewerLastName = detail.ReviewerLastName,
        //        ReviewerStatusId = detail.ReviewerStatusId,
        //        ReviewInstructions = detail.ReviewInstructions,
        //        StatusId = detail.StatusId,
        //    };
        //}


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
                //ReviewerStatus = (Enums.ProjectReviewerStatus)projectDetailCore.ReviewerStatusId,
                ReviewerStatusId = projectDetailCore.ReviewerStatusId,
                ReviewInstructions = projectDetailCore.ReviewInstructions,
                DetailStatus = (Enums.ProjectDetailStatus)projectDetailCore.StatusId,
                StatusId = projectDetailCore.StatusId,
                ReviewerComments = projectDetailCore.ReviewerComments,
            };
        }

        public List<ProjectViewModel> GetReviewerProjectsDone(Guid reviewerUserId)
        {
            IProjectRepo projectRepo = new ProjectRepo();
            var projectsCore = projectRepo.GetReviewerProjectsDone(reviewerUserId);
            var projectsView = BuildProjectsView(projectsCore);
            return projectsView;
        }
    }
}