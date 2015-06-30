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
        ProjectCoreModel GetProject(Guid userId, Guid projectId);

        void AddProject(ProjectCoreModel project);
        void AddProjectDetail(ProjectDetailCoreModel projectDetail);
        void UpdateProjectDetail(ProjectDetailCoreModel projectDetail);
        void DeleteProjectDetail(Guid guid, Guid projectId, Guid detailId);
        void AddProjectReviewer(ProjectDetailCoreModel projectDetail);
        void StartTheProject(Guid guid, Guid projectId);

        void ReviewerAccepted(Guid projectId, Guid reviewerUserId);

        //List<ProjectCoreModel> GetProjectsPendingReviewerAcceptance(Guid reviewerUserId);
        List<ProjectCoreModel> GetProjectsForReviewerByStatus(Guid reviewerUserId, Enums.ProjectStatus projectStatusId);

        void UpdateProjectDetailStatus(Guid guid, Guid projectDetailId, Enums.ProjectDetailStatus detailStatus);
        void ProjectCompletedByOwner(Guid userId, Guid projectId);
        List<ProjectCoreModel> GetReviewerProjectsInProgress(Guid reviewerUserId);

        ProjectDetailCoreModel GetProjectDetailById(Guid projectDetailId);
    }
}
