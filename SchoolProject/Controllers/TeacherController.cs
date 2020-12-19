
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using SchoolProject.Models.ViewModels;
using System.Diagnostics;


namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        //instantiate Teacher controller outside of each method: 
        private TeacherDataController teacherdatacontroller = new TeacherDataController();

        //need to also get related article data for this author to be displayed via view models 
        private CourseDataController coursedatacontroller = new CourseDataController();

        //also get related student data for teacher
        private StudentDataController studentdatacontroller = new StudentDataController();


        //GET : /Teacher/Error

        /// <summary>
        /// Shows specific errors for Teachers
        /// </summary>
        /// <returns></returns>

        public ActionResult Error()
        {
            return View();
        }

        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }


        //GET: /Teacher/List
        //method that links to specific page:  list.cshtml 
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //tries to get a list of authors.... 
                IEnumerable<Teacher> Teachers = teacherdatacontroller.ListTeachers(SearchKey);
                return View(Teachers);
            }
            catch ( Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLien(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //GET: /Teacher/Ajax_List
        public ActionResult Ajax_List()
        {
            return View();
        }



        //GET : /Teacher/Show/{id}
        //Set up for displaying the assigned class to a teacher
        public ActionResult Show(int id)
        {
            try
            {
                //View model connection here to show associated info (students and courses)
                ShowTeacher ViewModel = new ShowTeacher();

                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                IEnumerable<Course> CoursesTaught = coursedatacontroller.GetCoursesForTeacher(id);
                ViewModel.Teacher = SelectedTeacher;
                ViewModel.CoursesTaught = CoursesTaught;
                

                

                return View(ViewModel);  //viewmodel needs to pass through to the view
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {

                
                Teacher NewTeacher = teacherdatacontroller.FindTeacher(id);

                return View(NewTeacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
               
                teacherdatacontroller.DeleteTeacher(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }


        //GET: /Teacher/Ajax_New
        //refer to the TeacherDataController for connection to Cors in order to add new Teacher to SQL database
        //See teacherdatacontroller, teacher.js, Ajax_New.cshtml 
        public ActionResult Ajax_New()
        {     
            
            return View();
        }

   


        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname,  DateTime HireDate, decimal Salary) //not working so took out : string EmployeeNumber,
        {
            try
            {
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
               // NewTeacher.EmployeeNumber EmployeeNumber; //
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                teacherdatacontroller.AddTeacher(NewTeacher);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

           

            return RedirectToAction("List");
        }

        ///==============UPDATE =================================
        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the teacher and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Teacher/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);

                return View(SelectedTeacher);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            

        }


        public ActionResult Ajax_Update(int id)
        {
            try
            {
                Teacher SelectedTeacher = teacherdatacontroller.FindTeacher(id);
                return View(SelectedTeacher);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Receives a **POST*** request containing information about an existing teacher in the system, with new values. Conveys this information to the API, and redirects to the "Teacher Show" page of our updated teacher.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        /// <returns>A dynamic webpage which provides the current information of the teacher.</returns>
        /// <example>
        /// POST : /Teacher/Update/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Martha",
        ///	"TeacherLname":"Lee",
        ///	
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname)
        {
            try
            {
                Teacher TeacherInfo = new Teacher();
                TeacherInfo.TeacherFname = TeacherFname;
                TeacherInfo.TeacherLname = TeacherLname;

                teacherdatacontroller.UpdateTeacher(id, TeacherInfo);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Show/" + id);
        }

    }



}