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
    public class StudentDataController : ApiController
    {

        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext Teachers = new SchoolDbContext(); //1. new instance of a class as blog object


        //This Controller will access the students table of our blog database
        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>GET api/ArticleData/ListSttudents</example>
        /// <returns>A list of students</returns>
        /// 

        [HttpGet]
        public IEnumerable<Student> ListStudents()
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from students"; //accesses info from students table

            //Gather REsult Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Students List 
            List<Student> Students = new List<Student>();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                Student NewStudent = new Student();

                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                int StudentID = Convert.ToInt32(ResultSet["studentid"]);

                //NewStudent definitions
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentId = StudentID;

                //Student objects are added to NewStudent
                Students.Add(NewStudent);
            }

            //Close the connetion between MySQL Database and the WebServer
            Conn.Close();

            //Return final list of student
            return Students;
        }

        [HttpGet]
        [Route("api/StudentData/FindStudent/{studentid}")]
        public Student FindStudent(int studentid) //FindStudent links with the StudentController.cs 
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from students WHERE studentid=" + studentid; ; //accesses info from students table

            //Gather REsult Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Students List 
            Student NewStudent = new Student();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                int StudentID = Convert.ToInt32(ResultSet["studentid"]);

                //NewStudent definitions
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentId = StudentID;

               
            }

            return NewStudent; 

            
        }
    }

}
