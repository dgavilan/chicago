using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Infrastructure.Database.EF.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Infrastructure.Database.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        ProjectContext _projectContext;

        public ProjectRepo()
        {
            _projectContext = new ProjectContext();
        }

        public void AddProject(ProjectCoreModel project)
        {
            var timeStamp = DateTime.UtcNow;

            _projectContext.Projects.Add(new Project()
            {
                CreatedDate = timeStamp,
                DeletedDate = null,
                ModifiedDate = timeStamp,
                ProjectDescription = project.ProjectDescription,
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                UserId = project.UserId
            });

            _projectContext.SaveChanges();
        }

        public ProjectCoreModel GetProject(Guid userId, Guid projectId)
        {
            var projectDb = (from p in _projectContext.Projects
                             where p.UserId == userId
                             && p.ProjectId == projectId
                             select p).SingleOrDefault();

            return new ProjectCoreModel()
            {
                ProjectDescription = projectDb.ProjectDescription,
                ProjectId = projectDb.ProjectId,
                ProjectName = projectDb.ProjectName,
                UserId = projectDb.UserId
            };
        }

        public void AddProjectDetail(ProjectDetailCoreModel projectDetail)
        {
            var timeStamp = DateTime.UtcNow;

            var projectDetailDb = new ProjectDetail() 
            {
                CreatedBy = projectDetail.UserId,
                CreatedDate = timeStamp,
                DetailDescription = projectDetail.ProjectDetailDescription,
                DetailName = projectDetail.ProjectDetailName,
                DetailNumber = 0,
                ModifiedBy = projectDetail.UserId,
                ModifiedDate = timeStamp,
                ProjectDetailId = Guid.NewGuid(),
                ProjectId = projectDetail.ProjectId,
                UserId = projectDetail.UserId
            };

            _projectContext.ProjectDetails.Add(projectDetailDb);
            _projectContext.SaveChanges();
        }
    }
}
