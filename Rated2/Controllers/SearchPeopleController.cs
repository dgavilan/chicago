using Rated.Core.Contracts;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.Repository;
using Rated.Models.Project;
using Rated.Models.UserProfile;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rated.Web.Controllers
{
    public class SearchPeopleController : Controller
    {
        // GET: SearchPeople
        public ActionResult Index()
        {
            IUserRepo userRepo = new UserRepo();
            var users = userRepo.GetUsers();

            // Map from Core to ViewModel
            var profiles = new List<ProfileViewModel>();
            foreach (var user in users)
            {
                profiles.Add(new ProfileViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserId = user.UserId,
                });
            }

            return View(profiles);
        }

        [HttpPost]
        //public ActionResult Index(string FirstName, string lastName)
        public ActionResult Index(UserSearchCoreModel userSearch)
        {
            IUserRepo userRepo = new UserRepo();
            var users = userRepo.SearchUsers(userSearch);

            // Map from Core to ViewModel
            var profiles = new List<ProfileViewModel>();
            foreach (var user in users)
            {
                profiles.Add(new ProfileViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserId = user.UserId,
                });
            }

            return View(profiles);
        }

        [HttpGet]
        public ActionResult ProfileView(Guid userId)
        {
            //var projectRepo = new ProjectRepo();
            //var userRepo = new UserRepo();

            //var userProfile = userRepo.GetUserByUserId(userId);
            //userProfile.UserRating = projectRepo.GetUserRating(userId);

            //ViewBag.UserProfile = userProfile;

            //var projects = projectRepo.GetProjectsByUserId(userId);

            //foreach (var project in projects)
            //{
            //    project.ProjectRating = projectRepo.GetProjectRating(project.ProjectId);
            //}

            //var projectHelper = new ProjectHelper();

            //return View("ProjSummary", projectHelper.BuildProjectsView(projects));

            var projects = new List<ProjectViewModel>();

            var projectView = new ProjectViewModel()
            {
                ProjectName = "340B Web Rewrite Project",
                ProjectDescription = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
                OwnerFirstName = "Michael",
                OwnerLastName = "Dell",
                ProjectRating = 5,
                ProjectStatus = Core.Shared.Enums.ProjectStatus.Done,

                Company = new CompanyViewModel() {
                    CompanyName = "Manila Zoo",
                    City ="Schaumburg",
                    State = "IL",
                    CompanyDescription = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo"
                },

                Tasks = new List<TaskViewModel>() {
                    new TaskViewModel() {
                        TaskId = Guid.NewGuid(),
                        Name = "Design Application Architecture",
                        Description = "SSed ut perspiciatis unde omnis iste natus error perspiciatis unde omnis iste natus error perspiciatis unde omnis iste natus error perspiciatis unde omnis iste natus error perspiciatis unde omnis iste natus error perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
                        DetailStatus = Core.Shared.Enums.TaskStatus.Done,
                        TaskCount = 1,
                        HoursToComplete = 40,
                        StartDate = Convert.ToDateTime("1/1/2015"),
                        EndDate = Convert.ToDateTime("1/7/2015"),
                        Rating = 4.5M,

                        Reviewers = new List<ReviewerViewModel>() {
                            new ReviewerViewModel() {
                                ReviewerId = Guid.NewGuid(),
                                FirstName = "Steve",
                                LastName = "Jobs",
                                Rating = 4.5M,
                                Comments = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto.",
                                Role = "Project Manager"
                            },
                            new ReviewerViewModel() {
                                ReviewerId = Guid.NewGuid(),
                                FirstName = "Hillary",
                                LastName = "Clinton",
                                Rating = 4.5M,
                                Comments = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architect.",
                                Role = "Project Manager"
                            },
                        }
                    }, // task

                    new TaskViewModel() {
                        TaskId = Guid.NewGuid(),
                        Name = "Design Continuous Integration",
                        Description = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
                        DetailStatus = Core.Shared.Enums.TaskStatus.Done,
                        TaskCount = 1,
                        HoursToComplete = 40,
                        StartDate = Convert.ToDateTime("1/1/2015"),
                        EndDate = Convert.ToDateTime("1/7/2015"),
                        Rating = 4.5M,

                        Reviewers = new List<ReviewerViewModel>() {
                            new ReviewerViewModel() {
                                ReviewerId = Guid.NewGuid(),
                                FirstName = "Steve",
                                LastName = "Jobs",
                                Rating = 4.5M,
                                Comments = "SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto. SSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto.",
                                Role = "Project Manager"
                            },
                        }
                    }, // task
                },
            };

            projects.Add(projectView);

            return View("ProjSummary", projects);
        }

        public ActionResult ProjDetails_Orig(Guid projectId)
        {
            var projectRepo = new ProjectRepo();
            var userRepo = new UserRepo();

            var project = projectRepo.GetProjectWithDoneDetailsByProjectId(projectId);
            var user = userRepo.GetUserByUserId(project.UserId);

            project.OwnerFirstName = user.FirstName;
            project.OwnerLastName = user.LastName;
            project.ProjectRating = projectRepo.GetProjectRating(projectId);

            var projectHelper = new ProjectHelper();
            var projectView = projectHelper.BuildProjectView(project);

            projectView.Tasks = new List<Models.Project.TaskViewModel>();

            foreach (var detail in project.ProjectDetails)
            {
                var projDetail = projectHelper.BuildProjectDetailView(detail);

                if (detail.ReviewerUserId != Guid.Empty)
                {
                    var reviewerUser = userRepo.GetUserByUserId(detail.ReviewerUserId);
                    projDetail.ReviewerFirstName = reviewerUser.FirstName;
                    projDetail.ReviewerLastName = reviewerUser.LastName;
                }

                projectView.Tasks.Add(projDetail);
            }

            return View(projectView);
        }

        public ActionResult ProjDetails(Guid projectId)
        {
            return View();
            //var projectView = new ProjectViewModel() {
            //    ProjectName = "Project 1",
            //    ProjectDescription = "This is project 1",
            //    OwnerFirstName = "Michael",
            //    OwnerLastName = "Dell",
            //    ProjectRating = 5,
            //    ProjectStatus = Core.Shared.Enums.ProjectStatus.Done,

            //    Tasks = new List<TaskViewModel>() {
            //        new TaskViewModel() {
            //            TaskId = Guid.NewGuid(),
            //            Name = "Task 1",
            //            Description = "This is task 1",
            //            DetailStatus = Core.Shared.Enums.TaskStatus.Done,
            //            TaskCount = 1,
            //            HoursToComplete = 40,
            //            StartDate = Convert.ToDateTime("1/1/2015"),
            //            EndDate = Convert.ToDateTime("1/7/2015"),
            //            Rating = 4.5M,

            //            Reviewers = new List<ReviewerViewModel>() {
            //                new ReviewerViewModel() {
            //                    ReviewerId = Guid.NewGuid(),
            //                    FirstName = "Steve",
            //                    LastName = "Jobs",
            //                    Rating = 4.5M,
            //                    Comments = "This is great!",
            //                    Role = "Project Manager"
            //                },
            //                new ReviewerViewModel() {
            //                    ReviewerId = Guid.NewGuid(),
            //                    FirstName = "Hillary",
            //                    LastName = "Clinton",
            //                    Rating = 4.5M,
            //                    Comments = "When I'm president...",
            //                    Role = "Project Manager"
            //                },
            //            }
            //        },
            //    },
            //};

            //return View(projectView);
        }

    }
}