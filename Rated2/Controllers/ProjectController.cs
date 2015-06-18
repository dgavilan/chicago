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

            foreach(var project in projectsCore)
            {
                projectsView.Add(new ProjectViewModel() {
                    ProjectDescription = project.ProjectDescription,
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    CreatedDate = project.CreatedDate
                });
            }

            return View(projectsView);
        }

        //[HttpPost]
        //public ActionResult AddProject(string name, string description)
        //{
        //    return View();
        //}

        public ActionResult StartProject()
        {
            ViewBag.OpenAddProjectModal = 1;
            return View();
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
