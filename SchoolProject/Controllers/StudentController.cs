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


        //GET : /Student/New
        public ActionResult New()
        {
            return View();
        }


        //GET: /Article/Show/{id} <====== one article

        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();

            Student SelectedStudent = Controller.FindStudent(id);

            return View(SelectedStudent);  //think about the semantic elements - instead of "new" choose "selected" 
        }


        //GET: /Student/Update/{id}

        //need to go to the student data controller to get the info that we need to update
        // similar to the show above 
        public ActionResult Update(int id)
        {

            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(id);
            return View(SelectedStudent);
        }
        /// <summary>
        /// Recieves a POST request containing information about an existing student in the system, with new values
        /// Conveys this information to the API and redirects to the "Student Show" page of the updated Student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="StudentFname"></param>
        /// <param name="StudentLname"></param>
        /// <returns>A dynamic webpage which provides the current inforamtion of the Student</returns>
        /// <example> POST: /Student/Update/10
        /// FORM DATA / POST DATA / REQUEST BODY
        /// {
        /// "StudentFname": "Jane",
        /// "StudentLname": "Lee",
        /// 
        /// 
        /// 
        /// </example>
        /// 
        
        //POST: /Student/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string StudentFname, string StudentLname)
        {
            Student StudentInfo = new Student();
            StudentInfo.StudentFname = StudentFname;
            StudentInfo.StudentLname = StudentLname;

            StudentDataController controller = new StudentDataController();
            controller.UpdateStudent(id, StudentInfo);

            return RedirectToAction("Show" + id); //this allows us to redirect to the consequences of our actions 
        }
    }
}