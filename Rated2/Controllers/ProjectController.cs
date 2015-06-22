using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated2.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated2.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index(Enums.ProjectStatus projectType)
        {
            var projectRepo = new ProjectRepo();
            var userSession = new UserSession();
            var userId = userSession.GetUserSession().UserId;

            var projectsCore = projectRepo.GetProjectsByStatus(userId, projectType);

            var projectsView = new List<ProjectViewModel>();

            foreach (var project in projectsCore)
            {
                projectsView.Add(new ProjectViewModel()
                {
                    ProjectDescription = project.ProjectDescription,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    CreatedDate = project.CreatedDate,
                    ProjectDetailsCount = project.ProjectDetailsCount,
                    ProjectStatus = project.ProjectStatus
                });
            }

            return View(projectsView);
        }

        [HttpGet]
        public ActionResult StartProject()
        {
            var projectView = new ProjectViewModel() { 
                ProjectDetails = new List<ProjectDetailViewModel>()
            };

            ViewBag.OpenAddProjectModal = 1;
            return View(projectView);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var userSession = new UserSession();
                var user = userSession.GetUserSession();

            var projectRepo = new ProjectRepo();
            var projectCore = projectRepo.GetProject(user.UserId, id);
            var projectDetailsCore = projectRepo.GetProjectDetail(user.UserId, id);
            var detailView = new List<ProjectDetailViewModel>();

            foreach (var detail in projectDetailsCore)
            {
                detailView.Add(new ProjectDetailViewModel() { 
                    CreatedBy = detail.CreatedBy,
                    CreatedDate = detail.CreatedDate,
                    DetailCount = 0,
                    DetailDescription = detail.ProjectDetailDescription,
                    DetailName = detail.ProjectDetailName,
                    ModifiedBy = detail.ModifiedBy,
                    ModifiedDate = detail.ModifiedDate,
                    ProjectDetailId = detail.ProjectDetailId,
                    ProjectId = detail.ProjectId,
                    HoursToComplete = detail.HoursToComplete,
                    ReviewerFirstName = detail.ReviewerFirstName,
                    ReviewerLastName = detail.ReviewerLastName,
                    ReviewerEmail = detail.ReviewerEmail,
                    ReviewerStatusId = detail.ReviewerStatusId,
                    ReviewerFullName = (detail.ReviewerStatusId == (int)Enums.ProjectReviewerStatus.Sent) 
                        ? detail.ReviewerEmail
                        : detail.ReviewerFirstName + " " + detail.ReviewerLastName
                });
            }

            var projectView = new ProjectViewModel() {
                CreatedBy = projectCore.CreatedBy,
                CreatedDate = projectCore.CreatedDate,
                ModifiedBy = projectCore.ModifiedBy,
                ModifiedDate = projectCore.ModifiedDate,
                ProjectDescription = projectCore.ProjectDescription,
                ProjectDetailsCount = projectCore.ProjectDetailsCount,
                ProjectId = projectCore.ProjectId,
                ProjectDetails = detailView,
                ProjectName = projectCore.ProjectName,
                ProjectStatus = projectCore.ProjectStatus,
                ReviewerEmail = ""
            };

            ViewBag.OpenAddProjectModal = 0;

            return View("StartProject", projectView);
        }

        //// GET: Project/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Project/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Project/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Project/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Project/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Project/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Project/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
