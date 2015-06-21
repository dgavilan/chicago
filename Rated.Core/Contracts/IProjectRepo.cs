using Rated.Core.Models.Project;
using Rated.Core.Models.User;
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
    }
}
