using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rated.Web.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pending()
        {
            return View();
        }

        public ActionResult InProgress()
        {
            return View();
        }

        public ActionResult Completed()
        {
            return View();
        }
    }
}