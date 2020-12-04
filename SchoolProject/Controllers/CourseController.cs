using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course (not allowed to use "Class")
        public ActionResult Index()
        {
            return View();
        }

        //GET: //Course/List <=== many classes

        public ActionResult List()
        {
            CourseDataController Controller = new CourseDataController();
            IEnumerable<Course> Courses = Controller.ListCourses();

            return View(Courses);
        }

        //GET: /Class/Show/{id} <====== one class 

        public ActionResult Show(int id)
        {
            CourseDataController Controller = new CourseDataController();

            Course SelectedCourse = Controller.FindCourse(id);

            return View(SelectedCourse);
        }
    }
}