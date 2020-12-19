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

        //We can instantiate the Studentcontroller outside of each method
        private StudentDataController controller = new StudentDataController();

        //GET : /Student/Error
        /// <summary>
        /// This window is for showing Student Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET: //Student/List <=== many Students

        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //Get a list of students
               

                IEnumerable<Student> Students = Controller.ListStudents(SearchKey);
                return View(Students);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

           
        }


        //GET : /Student/New
        public ActionResult New()
        {
            return View();
        }


        //GET: /Article/Show/{id} <====== one article

        public ActionResult Show(int id)
        {
            try
            {
                

                Student SelectedStudent = Controller.FindStudent(id);

                return View(SelectedStudent);  //think about the semantic elements - instead of "new" choose "selected" 

            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
           
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

            StudentDataController Controller = new StudentDataController();
            Controller.UpdateStudent(id, StudentInfo);

            return RedirectToAction("Show" + id); //this allows us to redirect to the consequences of our actions 
        }
    }
}