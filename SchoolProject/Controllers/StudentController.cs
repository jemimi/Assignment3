using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET: //Article/List <=== many articles

        public ActionResult List(string StudentSearchKey)
        {
            //need to add using System Diagnositcs to use this debug line
            Debug.WriteLine("The search is " + StudentSearchKey);

            StudentDataController Controller = new StudentDataController();
            
            IEnumerable<Student> Students = Controller.ListStudents(StudentSearchKey);

            return View(Students);
        }

        //GET: /Article/Show/{id} <====== one article

        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();

            Student SelectedStudent = Controller.FindStudent(id);

            return View(SelectedStudent);
        }
    }
}