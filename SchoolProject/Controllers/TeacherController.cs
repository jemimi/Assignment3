
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;


namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        //GET: /Teacher/List
        //method that links to specific page:  list.cshtml 
        public ActionResult List()
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers();

            return View(Teachers);
        }

        
        //GET : /Teacher/Show/{id}
        //Set up for displaying the assigned class to a teacher
        public ActionResult Show(int id)
        {
            
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);
            //WebClass NewWebClass = controller.FindClass();  //assigned class

            return View(SelectedTeacher);
        }

       
        
    }
}