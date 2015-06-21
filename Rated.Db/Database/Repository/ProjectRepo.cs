using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.EF.Project;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                UserId = project.UserId,
                StatusId = (int)Enums.ProjectStatus.UnSent,
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
            var projectDetailDb = new ProjectDetail()
            {
                CreatedBy = projectDetail.CreatedBy,
                CreatedDate = projectDetail.CreatedDate,
                DetailDescription = projectDetail.ProjectDetailDescription,
                DetailName = projectDetail.ProjectDetailName,
                DetailNumber = projectDetail.DetailItemNumber,
                ModifiedBy = projectDetail.ModifiedBy,
                ModifiedDate = projectDetail.ModifiedDate,
                ProjectDetailId = projectDetail.ProjectDetailId,
                ProjectId = projectDetail.ProjectId,
                UserId = projectDetail.UserId,
                HoursToComplete = projectDetail.HoursToComplete
            };

            _projectContext.ProjectDetails.Add(projectDetailDb);
            _projectContext.SaveChanges();
        }

        public List<ProjectCoreModel> GetProjectsByStatus(Guid userId, Enums.ProjectStatus projectStatus)
        {
            var projectsDb = (from p in _projectContext.Projects
                              where p.UserId == userId
                              && p.StatusId == (int)projectStatus
                              orderby p.CreatedDate descending
                              select p).ToList();

            var projects = new List<ProjectCoreModel>();

            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel()
                {
                    ProjectDescription = project.ProjectDescription,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    Score = 0,
                    UserId = project.UserId,
                    CreatedDate = project.CreatedDate,
                    ProjectDetailsCount = project.ProjectDetails.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.StatusId
                });
            }

            return projects;
        }

        public List<ProjectDetailCoreModel> GetProjectDetail(Guid userId, Guid projectId)
        {
            var projectDetailsDb = (from pd in _projectContext.ProjectDetails
                                    where pd.UserId == userId
                                        && pd.ProjectId == projectId
                                    orderby pd.DetailNumber ascending
                                    select pd).ToList();

            var projectDetails = new List<ProjectDetailCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                projectDetails.Add(new ProjectDetailCoreModel()
                {
                    CreatedBy = detail.CreatedBy,
                    CreatedDate = detail.CreatedDate,
                    //DetailCount = 0,
                    ProjectDetailDescription = detail.DetailDescription,
                    ProjectDetailName = detail.DetailName,
                    ModifiedBy = detail.ModifiedBy,
                    ModifiedDate = detail.ModifiedDate,
                    ProjectDetailId = detail.ProjectDetailId,
                    ProjectId = detail.ProjectId,
                    HoursToComplete = detail.HoursToComplete
                });
            }

            return projectDetails;
        }

        public void UpdateProjectDetail(ProjectDetailCoreModel projectDetail)
        {
            var timeStamp = DateTime.UtcNow;

            var projectDetailDb = (from pd in _projectContext.ProjectDetails
                                   where pd.UserId == projectDetail.UserId
                                    && pd.ProjectDetailId == projectDetail.ProjectDetailId
                                   select pd).SingleOrDefault();

            projectDetailDb.DetailDescription = projectDetail.ProjectDetailDescription;
            projectDetailDb.DetailName = projectDetail.ProjectDetailName;
            projectDetailDb.ModifiedBy = projectDetail.UserId;
            projectDetailDb.ModifiedDate = timeStamp;
            projectDetailDb.HoursToComplete = projectDetail.HoursToComplete;

            _projectContext.Entry(projectDetailDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void DeleteProjectDetail(Guid userId, Guid projectId, Guid detailId)
        { 
            var projectDetailDb = (from pd in _projectContext.ProjectDetails
                                   where pd.UserId == userId
                                    && pd.ProjectId == projectId
                                    && pd.ProjectDetailId == detailId
                                   select pd).SingleOrDefault();

            _projectContext.Entry(projectDetailDb).State = EntityState.Deleted;
            _projectContext.SaveChanges();
        }
    }
}
