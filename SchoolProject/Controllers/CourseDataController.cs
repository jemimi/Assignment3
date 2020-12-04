using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;

namespace SchoolProject.Controllers
{
    public class CourseDataController : ApiController
    {

        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext(); //1. new instance of a class as blog object


        //This Controller will access the classes table of our blog database
        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>GET api/ArticleData/ListSttudents</example>
        /// <returns>A list of Classes</returns>
        /// 
        /// 
        /// 

        [HttpGet]
        public IEnumerable<Course> ListCourses()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from classes"; //accesses info from classes table

            //Gather REsult Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Courses List 
            List<Course> Courses = new List<Course>();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                Course NewCourse = new Course();

                string CourseName = ResultSet["classname"].ToString();
                string CourseCode = ResultSet["classcode"].ToString();
                int CourseID = Convert.ToInt32(ResultSet["classid"]);

                //NewClass definitions
                NewCourse.CourseName = CourseName;
                NewCourse.CourseCode = CourseCode;
                NewCourse.CourseId = CourseID;

                //Class objects are added to NewClass
                Courses.Add(NewCourse);
            }

            //Close the connetion between MySQL Database and the WebServer
            Conn.Close();

            //Return final list of Classes
            return Courses;
        }
       
        [HttpGet]
        [Route("api/CourseData/FindCourse/{courseid}")]
        public Course FindCourse(int courseid) //Findcourse links with the CourseController.cs 
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from Classes WHERE classid=" + courseid; ; //accesses info from classes table

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Classes List 
            Course NewCourse= new Course();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                string CourseName = ResultSet["classname"].ToString();
                string CourseCode = ResultSet["classcode"].ToString();
                int CourseID = Convert.ToInt32(ResultSet["classid"]);

                //NewCourse definitions
                NewCourse.CourseName = CourseName;
                NewCourse.CourseCode = CourseCode;
                NewCourse.CourseId = CourseID;


            }

            return NewCourse;


        }
    }

}
