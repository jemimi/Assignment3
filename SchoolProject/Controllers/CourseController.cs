using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class CourseController : Controller

    {
        //Instiantiate CourseController outside of each method - don't need to repeat it over and over
        private CourseDataController controller = new CourseDataController();
       


        // GET: /Course/Error (not allowed to use "Class")
        /// <summary>
        /// For specific errors 
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }

        //GET: //Course/List <=== many classes

        public ActionResult List(string SearchKey = null)
        {
            try //identifies code for which exceptions are activiated.
            {
                //try to get a list of Courses
                
                IEnumerable<Course> Courses = controller.ListCourses(SearchKey);

                return View(Courses);
            }
            catch (Exception ex) //catches an exception at the palce in a program where you want to handle problem
            //if something goes wrong, then error message will pop up
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex. Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //GET: /Class/Show/{id} <====== one class 

        public ActionResult Show(int id)
        {
            try
            {
                Course SelectedCourse = controller.FindCourse(id);
                return View(SelectedCourse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");

            }
            
        }

        //GET: /Course/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Course NewCourse = controller.FindCourse(id);
                return View(NewCourse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //POST: /Course/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                controller.DeleteCourse(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET: /Course/New
        public ActionResult New()
        {
            return View();
        }



        //POST: /Course/Create
        [HttpPost]
        public ActionResult Create(string CourseName, string CourseCode)
        {
            try
            {
                Course NewCourse = new Course();
                NewCourse.CourseName = CourseName;
                NewCourse.CourseCode = CourseCode;

                controller.AddCourse(NewCourse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("List");
            //To redirect to a different action which can be in the same or 
            //different controller. It tells ASP.NET MVC to respond with a 
            //browser to a different action instead of rendering HTML as 
            //View() method does
        }

        /// <summary>
        /// Routes to a dynamically generated "Course Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Course</param>
        /// <returns>A dynamic "Update Course" webpage which provides the current information of the Course and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Course/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Course SelectedCourse = controller.FindCourse(id);
                return View(SelectedCourse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //POST :/Course/Update
        [HttpPost]
        public ActionResult Update(int id, string CourseName, string CourseCode)
        {
            try
            {
                Course NewCourse = new Course();
                NewCourse.CourseName = CourseName;
                NewCourse.CourseCode = CourseCode;

                controller.UpdateCourse(id, NewCourse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }

    }
}