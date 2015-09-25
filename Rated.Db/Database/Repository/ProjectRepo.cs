using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.EF.Project;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
                StatusId = (int)project.ProjectStatus,
                CompanyId = project.CompanyId,
            });

            _projectContext.SaveChanges();
        }

        public ProjectCoreModel GetProjectForReviewerByProjectIdByUserId(Guid projectId, Guid reviewerUserId)
        {
            var projectDb = (from p in _projectContext.Projects
                             join u in _projectContext.Users on p.UserId equals u.UserId
                             where p.ProjectId == projectId
                             select new { p, u }).SingleOrDefault();

            return new ProjectCoreModel()
            {
                ProjectDescription = projectDb.p.ProjectDescription,
                ProjectId = projectDb.p.ProjectId,
                ProjectName = projectDb.p.ProjectName,
                UserId = projectDb.p.UserId,
                ProjectStatus = (Enums.ProjectStatus)projectDb.p.StatusId,
                CreatedDate = projectDb.p.CreatedDate,
                ModifiedDate = projectDb.p.ModifiedDate,
                OwnerFirstName = projectDb.u.FirstName,
                OwnerLastName = projectDb.u.LastName,
                ReviewerProjectStatus = (
                    (projectDb.p.Tasks.Where(
                        x => x.ReviewerUserId == reviewerUserId
                        && x.StatusId != (int)Enums.TaskStatus.Done
                    ).Any()) ? Enums.ReviewerProjectStatus.InProgress : Enums.ReviewerProjectStatus.Done)
            };
        }

        public ProjectCoreModel GetProjectWithDetailsByProjectId(Guid projectId)
        {
            var project = GetProjectByProjectId(projectId);
            project.ProjectDetails = GetProjectDetailsByProjectId(projectId);
            return project;
        }

        public ProjectCoreModel GetProjectWithDoneDetailsByProjectId(Guid projectId)
        {
            var project = GetProjectByProjectId(projectId);
            project.ProjectDetails = GetProjectDoneDetailsByProjectId(projectId);
            return project;
        }

        public ProjectCoreModel GetProjectByProjectId(Guid projectId)
        {
            var projectDb = (from p in _projectContext.Projects
                             //where p.UserId == userId
                             where p.ProjectId == projectId
                             select p).SingleOrDefault();

            return new ProjectCoreModel()
            {
                ProjectDescription = projectDb.ProjectDescription,
                ProjectId = projectDb.ProjectId,
                ProjectName = projectDb.ProjectName,
                UserId = projectDb.UserId,
                ProjectStatus = (Enums.ProjectStatus)projectDb.StatusId,
                CreatedDate = projectDb.CreatedDate,
                ModifiedDate = projectDb.ModifiedDate
            };
        }

        public void AddTask(TaskCoreModel task)
        {
            var projectDetailDb = new EF.Project.Task()
            {
                CreatedBy = task.CreatedBy,
                CreatedDate = task.CreatedDate,
                DetailDescription = task.Description,
                DetailName = task.Name,
                DetailNumber = task.TaskItemNumber,
                ModifiedBy = task.ModifiedBy,
                ModifiedDate = task.ModifiedDate,
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                UserId = task.UserId,
                HoursToComplete = task.HoursToComplete,
                StatusId = task.StatusId,
                ReviewInstructions = task.ReviewInstructions,
            };

            _projectContext.Tasks.Add(projectDetailDb);
            _projectContext.SaveChanges();
        }

        public List<ProjectCoreModel> GetProjectsForReviewerByStatus(Guid reviewerUserId, Enums.ProjectStatus projectStatusId)
        {
            var projectsDb = (from p in _projectContext.Projects
                              join u in _projectContext.Users on p.UserId equals u.UserId
                              join pr in _projectContext.ProjectReviewers on p.ProjectId equals pr.ProjectId
                              where pr.UserId == reviewerUserId
                                && p.StatusId == (int)projectStatusId
                              orderby p.CreatedDate descending
                              select new { p, OwnerFirstName = u.FirstName, OwnerLastName = u.LastName }).Distinct().ToList();

            var projects = new List<ProjectCoreModel>();

            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel()
                {
                    ProjectDescription = project.p.ProjectDescription,
                    ProjectId = project.p.ProjectId,
                    ProjectName = project.p.ProjectName,
                    ProjectRating = 0,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.Tasks.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId
                });
            }

            return projects;
        }

        public List<ProjectCoreModel> GetReviewerProjectsInProgress(Guid reviewerUserId)
        {
            var detailInProgress = new int[] { 
                (int)Enums.TaskStatus.ReviewerInProgressReviewingDetail, 
                (int)Enums.TaskStatus.ReviewerPendingAcceptance
            };

            var projectsDb = (from p in _projectContext.Projects
                              join u in _projectContext.Users on p.UserId equals u.UserId
                              join pd in _projectContext.Tasks on p.ProjectId equals pd.ProjectId
                              where p.StatusId == (int)Enums.ProjectStatus.InProgress
                                    && detailInProgress.Contains(pd.StatusId)
                                    && pd.ReviewerUserId == reviewerUserId
                              orderby p.CreatedDate descending
                              select new { p, OwnerFirstName = u.FirstName, OwnerLastName = u.LastName }).Distinct().ToList();

            var projects = new List<ProjectCoreModel>();

            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel()
                {
                    ProjectDescription = project.p.ProjectDescription,
                    ProjectId = project.p.ProjectId,
                    ProjectName = project.p.ProjectName,
                    ProjectRating = 0,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.Tasks.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId,

                    Company = new CompanyCoreModel(),
                });
            }

            return projects;
        }

        public List<ProjectCoreModel> GetProjectsByStatus(Guid userId, Enums.ProjectStatus projectStatus)
        {
            var projectsDb = (from p in _projectContext.Projects
                              join u in _projectContext.Users on p.UserId equals u.UserId
                              where p.UserId == userId
                                && p.StatusId == (int)projectStatus
                              orderby p.CreatedDate descending
                              select new { p, OwnerFirstName = u.FirstName, OwnerLastName = u.LastName }).ToList();

            var projects = new List<ProjectCoreModel>();

            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel()
                {
                    ProjectDescription = project.p.ProjectDescription,
                    ProjectId = project.p.ProjectId,
                    ProjectName = project.p.ProjectName,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.Tasks.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId,
                    ProjectRating = (project.p.Tasks.Count > 0) 
                        ? (Math.Round(project.p.Tasks.Average(x => x.DetailRating), 2))
                        : 0.00M,

                    Company = new CompanyCoreModel(),
                });
            }

            return projects;
        }

        public List<TaskCoreModel> GetProjectDoneDetailsByProjectId(Guid projectId)
        {
            var projectDetailsDb = (
                from pd in _projectContext.Tasks
                join u in _projectContext.Users on pd.ReviewerUserId equals u.UserId
                    into pru
                from u in pru.DefaultIfEmpty()
                where pd.ProjectId == projectId
                    && pd.StatusId == (int)Enums.TaskStatus.Done
                orderby pd.CreatedDate ascending
                select new
                {
                    pd,
                    ReviewerFirstName = u.FirstName,
                    ReviewerLastName = u.LastName,
                    ReviewerEmail = u.Email,
                }).ToList();

            var projectDetails = new List<TaskCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                var addThis = MapProjectDetailToCore(detail.pd);
                addThis.ReviewerFirstName = detail.ReviewerFirstName;
                addThis.ReviewerLastName = detail.ReviewerLastName;
                addThis.ReviewerEmail = detail.ReviewerEmail;

                projectDetails.Add(addThis);

            }

            return projectDetails;
        }

        public List<TaskCoreModel> GetProjectDetailsByProjectId(Guid projectId)
        {
            var projectDetailsDb = (
                from pd in _projectContext.Tasks
                join u in _projectContext.Users on pd.ReviewerUserId equals u.UserId
                    into pru
                from u in pru.DefaultIfEmpty()
                where pd.ProjectId == projectId
                orderby pd.CreatedDate ascending
                select new
                {
                    pd,
                    ReviewerFirstName = u.FirstName,
                    ReviewerLastName = u.LastName,
                    ReviewerEmail = u.Email,
                }).ToList();

            var projectDetails = new List<TaskCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                var addThis = MapProjectDetailToCore(detail.pd);
                addThis.ReviewerFirstName = detail.ReviewerFirstName;
                addThis.ReviewerLastName = detail.ReviewerLastName;
                addThis.ReviewerEmail = detail.ReviewerEmail;

                projectDetails.Add(addThis);
            }

            return projectDetails;
        }

        public List<TaskCoreModel> GetProjectDetailsByProjectIdByUserId(Guid projectId, Guid userId)
        {
            var projectDetailsDb = (
                from pd in _projectContext.Tasks
                join pr in _projectContext.ProjectReviewers on pd.TaskId equals pr.ProjectDetailId
                    into pdpr
                from pr in pdpr.DefaultIfEmpty()
                join u in _projectContext.Users on pr.UserId equals u.UserId
                    into pru
                from u in pru.DefaultIfEmpty()
                where pd.ProjectId == projectId
                    && pd.ReviewerUserId == userId
                orderby pd.DetailNumber ascending
                select new
                {
                    pd,
                    ReviewerFirstName = u.FirstName,
                    ReviewerLastName = u.LastName,
                    ReviewerEmail = u.Email,
                }).ToList();

            var projectDetails = new List<TaskCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                var addThis = MapProjectDetailToCore(detail.pd);
                addThis.ReviewerFirstName = detail.ReviewerFirstName;
                addThis.ReviewerLastName = detail.ReviewerLastName;
                addThis.ReviewerEmail = detail.ReviewerEmail;

                projectDetails.Add(addThis);
            }

            return projectDetails;
        }

        public List<TaskCoreModel> MapProjectDetailsToCore(List<EF.Project.Task> projectDetailsDb)
        {
            var projectDetails = new List<TaskCoreModel>();
            foreach (var detail in projectDetailsDb)
            {
                projectDetails.Add(MapProjectDetailToCore(detail));
            }
            return projectDetails;
        }

        private TaskCoreModel MapProjectDetailToCore(EF.Project.Task detail)
        {
            return new TaskCoreModel() {
                CreatedBy = detail.CreatedBy,
                CreatedDate = detail.CreatedDate,
                Description = detail.DetailDescription,
                Name = detail.DetailName,
                ModifiedBy = detail.ModifiedBy,
                ModifiedDate = detail.ModifiedDate,
                TaskId = detail.TaskId,
                ProjectId = detail.ProjectId,
                HoursToComplete = detail.HoursToComplete,
                StatusId = detail.StatusId,
                ReviewInstructions = detail.ReviewInstructions,
                Status = (Enums.TaskStatus)detail.StatusId,
                ReviewerUserId = detail.ReviewerUserId,
                Rating = detail.DetailRating,
                ReviewerComments = detail.ReviewerComments,
            };
        }

        public void UpdateProjectDetail(TaskCoreModel projectDetail)
        {
            var timeStamp = DateTime.UtcNow;

            var projectDetailDb = (from pd in _projectContext.Tasks
                                   where pd.TaskId == projectDetail.TaskId
                                   select pd).SingleOrDefault();

            projectDetailDb.DetailDescription = projectDetail.Description;
            projectDetailDb.DetailName = projectDetail.Name;
            projectDetailDb.ModifiedBy = projectDetail.UserId;
            projectDetailDb.ModifiedDate = timeStamp;
            projectDetailDb.HoursToComplete = projectDetail.HoursToComplete;
            projectDetailDb.ReviewInstructions = projectDetail.ReviewInstructions;
            projectDetailDb.StatusId = projectDetail.StatusId;
            projectDetailDb.DetailRating = projectDetail.Rating;
            projectDetailDb.ReviewerComments = projectDetail.ReviewerComments;

            _projectContext.Entry(projectDetailDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void DeleteProjectDetail(Guid userId, Guid projectId, Guid detailId)
        {
            var projectDetailDb = (from pd in _projectContext.Tasks
                                   where pd.UserId == userId
                                    && pd.ProjectId == projectId
                                    && pd.TaskId == detailId
                                   select pd).SingleOrDefault();

            _projectContext.Entry(projectDetailDb).State = EntityState.Deleted;
            _projectContext.SaveChanges();
        }

        public void AddProjectReviewer(TaskCoreModel projectDetail)
        {
            try
            {
                // 1. insert record in user table if user id doesn't exist

                var projectReviewer = (from u in _projectContext.Users
                                       where u.Email == projectDetail.ReviewerEmail
                                       select u).SingleOrDefault();

                var reviewerUserId = Guid.Empty;
                var timeStamp = DateTime.UtcNow;

                if (projectReviewer == null)
                {
                    reviewerUserId = Guid.NewGuid();
                    _projectContext.Users.Add(new User()
                    {
                        Bio = "",
                        CreatedDate = timeStamp,
                        Email = projectDetail.ReviewerEmail,
                        FirstName = "",
                        IsActive = false,
                        LastLoginDate = Convert.ToDateTime("1/1/1990"),
                        LastName = "",
                        ModifiedDate = timeStamp,
                        Password = "",
                        UserId = reviewerUserId,
                        StatusId = (int)Enums.UserStatus.PendingReviewerAcceptance
                    });

                    _projectContext.SaveChanges();
                }
                else
                {
                    reviewerUserId = projectReviewer.UserId;
                }

                // 2. insert record in ProjectReviewer table

                if (projectDetail.TaskId == Guid.Empty)
                {
                    // NOTE: Add reviewer to all details
                    var detailsDb = (from d in _projectContext.Tasks
                                     where d.ProjectId == projectDetail.ProjectId
                                     select d).ToList();

                    foreach (var detail in detailsDb)
                    {
                        // NOTE: change detail status to ReviewerPendingAcceptance
                        detail.ReviewerUserId = reviewerUserId;
                        detail.StatusId = (int)Enums.TaskStatus.ReviewerPendingAcceptance;
                        _projectContext.Entry(detail).State = EntityState.Modified;
                    }// foreach
                }
                else
                {
                    var oneDetailDb = (from d in _projectContext.Tasks
                                       where d.TaskId == projectDetail.TaskId
                                       select d).SingleOrDefault();

                    oneDetailDb.ReviewerUserId = reviewerUserId;
                    oneDetailDb.StatusId = (int)Enums.TaskStatus.ReviewerPendingAcceptance;
                    _projectContext.Entry(oneDetailDb).State = EntityState.Modified;
                }

                // 3. Change project status to In Progress
                var projectDb = (from p in _projectContext.Projects
                                     where p.ProjectId == projectDetail.ProjectId
                                     select p).SingleOrDefault();

                projectDb.StatusId = (int)Enums.ProjectStatus.InProgress;

                _projectContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.Append(String.Format("{0} - Error:{1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(errors.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void StartTheProject(Guid userId, ProjectCoreModel projectCore)
        {
            var projectDb = (from p in _projectContext.Projects
                             where p.UserId == userId
                             && p.ProjectId == projectCore.ProjectId
                             select p).SingleOrDefault();

            projectDb.StatusId = (int)Enums.ProjectStatus.ReviewerPendingAcceptance;
            projectDb.ModifiedDate = DateTime.UtcNow;

            _projectContext.Entry(projectDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void ReviewerAccepted(ProjectCoreModel projectCore)
        {
            var timeStamp = DateTime.UtcNow;

            var projectDb = (from p in _projectContext.Projects
                             where p.ProjectId == projectCore.ProjectId
                             select p).SingleOrDefault();

            projectDb.StatusId = (int)projectCore.ProjectStatus;
            projectDb.ModifiedDate = timeStamp;
            _projectContext.Entry(projectDb).State = EntityState.Modified;

            var projectReviewerDb = (from pr in _projectContext.ProjectReviewers
                                     where pr.ProjectId == projectCore.ProjectId
                                     && pr.UserId == projectCore.UserId
                                     select pr).SingleOrDefault();

            projectReviewerDb.StatusId = (int)Enums.UserStatus.ReviewerAccepted;
            projectReviewerDb.ModifiedBy = projectCore.UserId;
            projectReviewerDb.ModifiedDate = timeStamp;
            _projectContext.Entry(projectReviewerDb).State = EntityState.Modified;

            _projectContext.SaveChanges();
        }

        public void UpdateProjectDetailStatus(Guid userId, Guid projectDetailId, Enums.TaskStatus detailStatus)
        {
            var detailDb = (from d in _projectContext.Tasks
                             where d.TaskId == projectDetailId
                             select d).SingleOrDefault();

            detailDb.StatusId = (int)detailStatus;
            detailDb.ModifiedDate = DateTime.UtcNow;
            detailDb.ModifiedBy = userId;

            _projectContext.Entry(detailDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void ProjectCompletedByOwner(Guid userId, Guid projectId)
        {
            var hasTasksInReview = (from d in _projectContext.Tasks
                                    where d.ProjectId == projectId
                                    && d.StatusId == (int)Enums.TaskStatus.ReviewerInProgressReviewingDetail
                                    select d.TaskId).Any();

            if (!hasTasksInReview)
            { 
                var projectDb = (from p in _projectContext.Projects
                                     where p.ProjectId == projectId
                                     select p).SingleOrDefault();

                projectDb.StatusId = (int)Enums.ProjectStatus.ReviewerInProgressReviewingProject;
                projectDb.ModifiedDate = DateTime.UtcNow;

                _projectContext.Entry(projectDb).State = EntityState.Modified;
                _projectContext.SaveChanges();
            }
        }

        public TaskCoreModel GetProjectDetailById(Guid projectDetailId)
        { 
            var projectDetailDb = (from pd in _projectContext.Tasks
                                       where pd.TaskId == projectDetailId
                                       select pd).SingleOrDefault();

            return MapProjectDetailToCore(projectDetailDb);
        }

        public ProjectCount GetProjectCounts(Guid userId)
        {
            var reviewInProgress = new int[] { 
                (int)Enums.TaskStatus.ReviewerInProgressReviewingDetail, 
                (int)Enums.TaskStatus.ReviewerPendingAcceptance
            };

            var projectsDb = (from p in _projectContext.Projects
                                  where p.UserId == userId
                                  select p).ToList();

            var projectsReviewDb = (from p in _projectContext.Projects
                                    join pd in _projectContext.Tasks on p.ProjectId equals pd.ProjectId
                                    where pd.ReviewerUserId == userId
                                    select new { ProjectId = pd.ProjectId, ProjectStatusId = pd.StatusId }).Distinct().ToList();

            return new ProjectCount() {
                ProjectDraft = projectsDb.Where(x => x.StatusId == (int)Enums.ProjectStatus.Draft).Count(),
                //ProjectInProgress = projectsDb.Where(x => pendingStatuses.Contains(x.StatusId)).Count(),
                ProjectInProgress = projectsDb.Where(x => x.StatusId == (int)Enums.ProjectStatus.InProgress).Count(),
                //ProjectPending = projectsDb.Where(x => x.StatusId == (int)Enums.ProjectStatus.ReviewerPendingAcceptance).Count(),
                ProjectDone = projectsDb.Where(x => x.StatusId == (int)Enums.ProjectStatus.Done).Count(),

                ReviewInProgress = projectsReviewDb.Where(x => reviewInProgress.Contains(x.ProjectStatusId)).Count(),
                ReviewDone = projectsReviewDb.Where(x => x.ProjectStatusId == (int)Enums.TaskStatus.Done).Count(),
            };
        }

        public void MoveToNextProjectStatus(ProjectCoreModel projectCore)
        {
            var projectDb = (from p in _projectContext.Projects
                              where p.ProjectId == projectCore.ProjectId
                              select p).SingleOrDefault();

            projectDb.StatusId = (int)projectCore.ProjectStatus;
            projectDb.ModifiedDate = DateTime.UtcNow;

            _projectContext.Entry(projectDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void SetProjectToDone(Guid projectId)
        { 
            var projectHasUnreviewedDetails = (from pd in _projectContext.Tasks
                                                where pd.ProjectId == projectId
                                                    && pd.StatusId != (int)Enums.TaskStatus.Done
                                                select pd.TaskId).Any();

            if (!projectHasUnreviewedDetails)
            { 
                // NOTE: All details have been reviewed. Set project status to done.
                var projectDb = (from p in _projectContext.Projects
                                     where p.ProjectId == projectId
                                     select p).SingleOrDefault();

                projectDb.StatusId = (int)Enums.ProjectStatus.Done;
                projectDb.ModifiedDate = DateTime.UtcNow;

                _projectContext.Entry(projectDb).State = EntityState.Modified;
                _projectContext.SaveChanges();
            }
        }

        public List<ProjectCoreModel> GetReviewerProjectsDone(Guid reviewerUserId)
        {
            var projectsDb = (from p in _projectContext.Projects
                              join u in _projectContext.Users on p.UserId equals u.UserId
                              join pd in _projectContext.Tasks on p.ProjectId equals pd.ProjectId
                              where pd.StatusId == (int)Enums.TaskStatus.Done
                                  && pd.ReviewerUserId == reviewerUserId
                              orderby p.CreatedDate descending
                              select new { p, OwnerFirstName = u.FirstName, OwnerLastName = u.LastName }).Distinct().ToList();

            var projects = new List<ProjectCoreModel>();

            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel()
                {
                    ProjectDescription = project.p.ProjectDescription,
                    ProjectId = project.p.ProjectId,
                    ProjectName = project.p.ProjectName,
                    ProjectRating = 0,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.Tasks.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId
                });
            }

            return projects;
        }

        public decimal GetUserRating(Guid userId)
        {
            var projectsDb = (from p in _projectContext.Projects
                                where p.UserId == userId
                                && p.StatusId != (int)Enums.ProjectStatus.Draft
                                select p).ToList();

            var userRating = new List<decimal>();
            foreach(var project in projectsDb)
            {
                var details = (project.Tasks.Where(x => x.StatusId == (int)Enums.TaskStatus.Done)).ToList();
                if (details.Count() > 0)
                {
                    userRating.Add(details.Average(x => x.DetailRating));
                }
            } // foreach

            var rating = Math.Round((userRating.Count() == 0) ? 0 : userRating.Average(x => x),2);
            return rating;
        }

        public decimal GetProjectRating(Guid projectId)
        {
            var projectDb = (from p in _projectContext.Projects
                              where p.StatusId != (int)Enums.ProjectStatus.Draft
                                && p.ProjectId == projectId
                              select p).SingleOrDefault();

            var details = (projectDb.Tasks.Where(x => x.StatusId == (int)Enums.TaskStatus.Done)).ToList();
            return details.Average(x => x.DetailRating);
        }

        public List<ProjectCoreModel> GetProjectsByUserId(Guid userId)
        {
            var projectsDb = (from p in _projectContext.Projects
                                  join pd in _projectContext.Tasks on p.ProjectId equals pd.ProjectId
                                  join c in _projectContext.Companies on p.CompanyId equals c.CompanyId
                                  where p.UserId == userId
                                  && pd.StatusId == (int)Enums.TaskStatus.Done
                                  select p).Distinct().ToList();

            var projects = new List<ProjectCoreModel>();
            foreach (var project in projectsDb)
            {
                projects.Add(new ProjectCoreModel() {
                    ProjectDescription = project.ProjectDescription,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectRating = 0,
                    UserId = project.UserId,
                    CreatedDate = project.CreatedDate,
                    ModifiedDate = project.ModifiedDate,
                    ProjectDetailsCount = project.Tasks.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.StatusId,
                    Company = new CompanyCoreModel() {
                        Address1 = project.Company.Address1,
                        Address2 = project.Company.Address2,
                        City = project.Company.City,
                        Description = project.Company.CompanyDescription,
                        CompanyId = project.Company.CompanyId,
                        Name = project.Company.CompanyName,
                        CreatedDate = project.Company.CreatedDate,
                        ModifiedDate = project.Company.ModifiedDate,
                        State = project.Company.State,
                        Zip = project.Company.Zip,
                    }
                });
            }

            return projects;
        }
    }
}
