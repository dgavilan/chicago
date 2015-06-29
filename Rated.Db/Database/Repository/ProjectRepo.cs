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
                StatusId = (int)Enums.ProjectStatus.Draft,
            });

            _projectContext.SaveChanges();
        }

        public ProjectCoreModel GetProjectByProjectId(Guid projectId)
        {
            var projectDb = (from p in _projectContext.Projects

                             //join pr in _projectContext.ProjectReviewers on p.ProjectId equals pr.ProjectId

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
            };
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
                UserId = projectDb.UserId,
                ProjectStatus = (Enums.ProjectStatus)projectDb.StatusId,
                CreatedDate = projectDb.CreatedDate,
                ModifiedDate = projectDb.ModifiedDate
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
                HoursToComplete = projectDetail.HoursToComplete,
                StatusId = projectDetail.StatusId,
            };

            _projectContext.ProjectDetails.Add(projectDetailDb);
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
                    Score = 0,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.ProjectDetails.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId
                });
            }

            return projects;
        }

        //public List<ProjectCoreModel> GetProjectsPendingReviewerAcceptance(Guid reviewerUserId)
        //{
        //    var projectsDb = (from p in _projectContext.Projects
        //                      join u in _projectContext.Users on p.UserId equals u.UserId
        //                      join pr in _projectContext.ProjectReviewers on p.ProjectId equals pr.ProjectId
        //                      where pr.UserId == reviewerUserId
        //                        && p.StatusId == (int)Enums.ProjectStatus.WaitingApproverAcceptance
        //                      orderby p.CreatedDate descending
        //                      select new { p, OwnerFirstName = u.FirstName, OwnerLastName = u.LastName }).ToList();

        //    var projects = new List<ProjectCoreModel>();

        //    foreach (var project in projectsDb)
        //    {
        //        projects.Add(new ProjectCoreModel()
        //        {
        //            ProjectDescription = project.p.ProjectDescription,
        //            ProjectId = project.p.ProjectId,
        //            ProjectName = project.p.ProjectName,
        //            Score = 0,
        //            UserId = project.p.UserId,
        //            OwnerFirstName = project.OwnerFirstName,
        //            OwnerLastName = project.OwnerLastName,
        //            CreatedDate = project.p.CreatedDate,
        //            ModifiedDate = project.p.ModifiedDate,
        //            ProjectDetailsCount = project.p.ProjectDetails.Count(),
        //            ProjectStatus = (Enums.ProjectStatus)project.p.StatusId
        //        });
        //    }

        //    return projects;
        //}

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
                    Score = 0,
                    UserId = project.p.UserId,
                    OwnerFirstName = project.OwnerFirstName,
                    OwnerLastName = project.OwnerLastName,
                    CreatedDate = project.p.CreatedDate,
                    ModifiedDate = project.p.ModifiedDate,
                    ProjectDetailsCount = project.p.ProjectDetails.Count(),
                    ProjectStatus = (Enums.ProjectStatus)project.p.StatusId
                });
            }

            return projects;
        }

        public List<ProjectDetailCoreModel> GetProjectDetail(Guid userId, Guid projectId)
        {
            var projectDetailsDb = (from pd in _projectContext.ProjectDetails
                                    join pr in _projectContext.ProjectReviewers on pd.ProjectDetailId equals pr.ProjectDetailId
                                        into pdpr
                                    from pr in pdpr.DefaultIfEmpty()
                                    join u in _projectContext.Users on pr.UserId equals u.UserId
                                        into pru
                                    from u in pru.DefaultIfEmpty()
                                    where pd.UserId == userId
                                        && pd.ProjectId == projectId
                                    orderby pd.DetailNumber ascending
                                    select new
                                    {
                                        pd,
                                        ReviewerFirstName = u.FirstName,
                                        ReviewerLastName = u.LastName,
                                        ReviewerEmail = u.Email,
                                        ReviewerStatusId = (pr != null)
                                            ? pr.StatusId
                                            : (int)Enums.ProjectReviewerStatus.NeedsReviewer,
                                    }).ToList();

            var projectDetails = new List<ProjectDetailCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                projectDetails.Add(new ProjectDetailCoreModel()
                {
                    CreatedBy = detail.pd.CreatedBy,
                    CreatedDate = detail.pd.CreatedDate,
                    ProjectDetailDescription = detail.pd.DetailDescription,
                    ProjectDetailName = detail.pd.DetailName,
                    ModifiedBy = detail.pd.ModifiedBy,
                    ModifiedDate = detail.pd.ModifiedDate,
                    ProjectDetailId = detail.pd.ProjectDetailId,
                    ProjectId = detail.pd.ProjectId,
                    HoursToComplete = detail.pd.HoursToComplete,
                    ReviewerFirstName = detail.ReviewerFirstName,
                    ReviewerLastName = detail.ReviewerLastName,
                    ReviewerEmail = detail.ReviewerEmail,
                    ReviewerStatusId = detail.ReviewerStatusId,
                    HasReviewer = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.Accepted) ? false : true,
                    StatusId = detail.pd.StatusId,
                });
            }

            return projectDetails;
        }

        public List<ProjectDetailCoreModel> GetProjectDetailByProjectId(Guid projectId)
        {
            var projectDetailsDb = (from pd in _projectContext.ProjectDetails
                                    join pr in _projectContext.ProjectReviewers on pd.ProjectDetailId equals pr.ProjectDetailId
                                        into pdpr
                                    from pr in pdpr.DefaultIfEmpty()
                                    join u in _projectContext.Users on pr.UserId equals u.UserId
                                        into pru
                                    from u in pru.DefaultIfEmpty()
                                    where pd.ProjectId == projectId
                                    orderby pd.DetailNumber ascending
                                    select new
                                    {
                                        pd,
                                        ReviewerFirstName = u.FirstName,
                                        ReviewerLastName = u.LastName,
                                        ReviewerEmail = u.Email,
                                        ReviewerStatusId = (pr != null)
                                            ? pr.StatusId
                                            : (int)Enums.ProjectReviewerStatus.NeedsReviewer,
                                    }).ToList();

            var projectDetails = new List<ProjectDetailCoreModel>();

            foreach (var detail in projectDetailsDb)
            {
                projectDetails.Add(new ProjectDetailCoreModel()
                {
                    CreatedBy = detail.pd.CreatedBy,
                    CreatedDate = detail.pd.CreatedDate,
                    ProjectDetailDescription = detail.pd.DetailDescription,
                    ProjectDetailName = detail.pd.DetailName,
                    ModifiedBy = detail.pd.ModifiedBy,
                    ModifiedDate = detail.pd.ModifiedDate,
                    ProjectDetailId = detail.pd.ProjectDetailId,
                    ProjectId = detail.pd.ProjectId,
                    HoursToComplete = detail.pd.HoursToComplete,
                    ReviewerFirstName = detail.ReviewerFirstName,
                    ReviewerLastName = detail.ReviewerLastName,
                    ReviewerEmail = detail.ReviewerEmail,
                    ReviewerStatusId = detail.ReviewerStatusId,
                    HasReviewer = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.Accepted) ? false : true,
                    StatusId = detail.pd.StatusId,
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

        public void AddProjectReviewer(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                // 1. insert record in user table if user id doesn't exist

                var projectReviewer = (from u in _projectContext.Users
                                       where u.Email == projectDetail.ReviewerEmail
                                       select u).SingleOrDefault();

                var reviewerId = Guid.Empty;
                var timeStamp = DateTime.UtcNow;

                if (projectReviewer == null)
                {
                    reviewerId = Guid.NewGuid();
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
                        UserId = reviewerId,
                        StatusId = (int)Enums.UserStatus.PendingReviewerAcceptance
                    });

                    _projectContext.SaveChanges();
                }
                else
                {
                    reviewerId = projectReviewer.UserId;
                }

                // 2. insert record in ProjectReviewer table

                if (projectDetail.ProjectDetailId == Guid.Empty)
                {
                    // NOTE: Add reviewer to all details
                    var detailsDb = (from d in _projectContext.ProjectDetails
                                     where d.ProjectId == projectDetail.ProjectId
                                     select d).ToList();
                    foreach (var detail in detailsDb)
                    {
                        _projectContext.ProjectReviewers.Add(new ProjectReviewer()
                        {
                            ProjectReviewerId = Guid.NewGuid(),
                            CreatedBy = projectDetail.UserId,
                            CreatedDate = timeStamp,
                            ModifiedBy = projectDetail.UserId,
                            ModifiedDate = timeStamp,
                            ProjectDetailId = detail.ProjectDetailId,
                            ProjectId = projectDetail.ProjectId,
                            UserId = reviewerId,
                            StatusId = (int)Enums.ProjectReviewerStatus.Sent
                        });
                    }
                }
                else
                {
                    // NOTE: Only assign reviewer to 1 detail
                    _projectContext.ProjectReviewers.Add(new ProjectReviewer()
                    {
                        ProjectReviewerId = Guid.NewGuid(),
                        CreatedBy = projectDetail.UserId,
                        CreatedDate = timeStamp,
                        ModifiedBy = projectDetail.UserId,
                        ModifiedDate = timeStamp,
                        ProjectDetailId = projectDetail.ProjectDetailId,
                        ProjectId = projectDetail.ProjectId,
                        UserId = reviewerId,
                        StatusId = (int)Enums.ProjectReviewerStatus.Sent
                    });
                }

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

        public void StartTheProject(Guid userId, Guid projectId)
        {
            var projectDb = (from p in _projectContext.Projects
                             where p.UserId == userId
                             && p.ProjectId == projectId
                             select p).SingleOrDefault();

            projectDb.StatusId = (int)Enums.ProjectStatus.WaitingApproverAcceptance;
            projectDb.ModifiedDate = DateTime.UtcNow;

            _projectContext.Entry(projectDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void ReviewerAccepted(Guid projectId, Guid reviewerUserId)
        {
            var timeStamp = DateTime.UtcNow;

            var projectDb = (from p in _projectContext.Projects
                             where p.ProjectId == projectId
                             select p).SingleOrDefault();

            projectDb.StatusId = (int)Enums.ProjectStatus.InProgressWaitingForOwnerToFinishProject;
            //projectDb.ModifiedBy = reviewerUserId;
            projectDb.ModifiedDate = timeStamp;
            _projectContext.Entry(projectDb).State = EntityState.Modified;

            var projectReviewerDb = (from pr in _projectContext.ProjectReviewers
                                     where pr.ProjectId == projectId
                                     && pr.UserId == reviewerUserId
                                     select pr).SingleOrDefault();

            projectReviewerDb.StatusId = (int)Enums.UserStatus.ReviewerAccepted;
            projectReviewerDb.ModifiedBy = reviewerUserId;
            projectReviewerDb.ModifiedDate = timeStamp;
            _projectContext.Entry(projectReviewerDb).State = EntityState.Modified;

            _projectContext.SaveChanges();
        }

        public void UpdateProjectDetailStatus(Guid userId, Guid projectDetailId, Enums.ProjectDetailStatus detailStatus)
        {
            var detailDb = (from d in _projectContext.ProjectDetails
                             where d.ProjectDetailId == projectDetailId
                             select d).SingleOrDefault();

            detailDb.StatusId = (int)detailStatus;
            detailDb.ModifiedDate = DateTime.UtcNow;
            detailDb.ModifiedBy = userId;

            _projectContext.Entry(detailDb).State = EntityState.Modified;
            _projectContext.SaveChanges();
        }

        public void MarkProjectAsComplete(Guid userId, Guid projectId)
        {
            var hasTasksInReview = (from d in _projectContext.ProjectDetails
                                    where d.ProjectId == projectId
                                    && d.StatusId == (int)Enums.ProjectDetailStatus.InReview
                                    select d.ProjectDetailId).Any();

            if (!hasTasksInReview)
            { 
                var projectDb = (from p in _projectContext.Projects
                                     where p.ProjectId == projectId
                                     select p).SingleOrDefault();

                projectDb.StatusId = (int)Enums.ProjectStatus.OwnerHasCompletedAllTasks;
                projectDb.ModifiedDate = DateTime.UtcNow;

                _projectContext.Entry(projectDb).State = EntityState.Modified;
                _projectContext.SaveChanges();
            }
        }

    }
}
