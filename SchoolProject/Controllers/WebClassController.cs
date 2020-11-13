using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class WebClassController : Controller
    {
        // GET: WebClass (not allowed to use "Class")
        public ActionResult Index()
        {
            return View();
        }

        //GET: //WebClass/List <=== many classes

        public ActionResult List()
        {
            WebClassDataController Controller = new WebClassDataController();
            IEnumerable<WebClass> WebClasses = Controller.ListWebClasses();

            return View(WebClasses);
        }

        //GET: /Class/Show/{id} <====== one class 

        public ActionResult Show(int id)
        {
            WebClassDataController Controller = new WebClassDataController();

            WebClass SelectedWebClass = Controller.FindWebClass(id);

            return View(SelectedWebClass);
        }
    }
}