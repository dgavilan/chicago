using Rated.Core.Models.Project;
using Rated.Core.Models.User;
using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Contracts
{
    public interface IProjectRepo
    {
        ProjectCoreModel GetProjectByProjectId(Guid projectId);

        void AddProject(ProjectCoreModel project);
        void AddProjectDetail(ProjectDetailCoreModel projectDetail);
        void UpdateProjectDetail(ProjectDetailCoreModel projectDetail);
        void DeleteProjectDetail(Guid guid, Guid projectId, Guid detailId);
        void AddProjectReviewer(ProjectDetailCoreModel projectDetail);
        void StartTheProject(Guid guid, ProjectCoreModel projectCore);


        //List<ProjectCoreModel> GetProjectsPendingReviewerAcceptance(Guid reviewerUserId);
        List<ProjectCoreModel> GetProjectsForReviewerByStatus(Guid reviewerUserId, Enums.ProjectStatus projectStatusId);

        void UpdateProjectDetailStatus(Guid guid, Guid projectDetailId, Enums.ProjectDetailStatus detailStatus);
        void ProjectCompletedByOwner(Guid userId, Guid projectId);
        List<ProjectCoreModel> GetReviewerProjectsInProgress(Guid reviewerUserId);

        ProjectDetailCoreModel GetProjectDetailById(Guid projectDetailId);
        ProjectCount GetProjectCounts(Guid userId);

        void ReviewerAccepted(ProjectCoreModel projectCore);
        void MoveToNextProjectStatus(ProjectCoreModel projectCore);

        void SetProjectToDone(Guid projectId);

        List<ProjectCoreModel> GetReviewerProjectsDone(Guid reviewerUserId);

        decimal GetUserRating(Guid userId);
    }
}
