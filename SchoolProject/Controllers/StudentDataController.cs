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
        private SchoolDbContext School = new SchoolDbContext(); //1. new instance of a class as blog object


        //This Controller will access the students table of our blog database
        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>GET api/ArticleData/ListSttudents</example>
        /// <returns>A list of articles</returns>
        /// 

        ///<summary>
        /// returns the search results when a words is typed in the search box
        /// </summary>
        /// <param name="StudentSearchKey"></param>
        /// <returns>returns search results </returns>

        [HttpGet]
        //Need to configure the route so that the student search key can be accessed by data access level
        //{StudentSearchKey} is in reference to the form in Views>Student>list.cshtml 
        [Route("api/StudentData/ListStudents/{StudentSearchKey}")]
        public IEnumerable<Student> ListStudents(string StudentSearchKey) //Student is a class from the models.article.cs
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            //accesses info from SQL students table
            string query = "select * from students where studentfname like @searchkey or studentlname like @searchkey";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@SearchKey", "%" + StudentSearchKey + "%"); // partial query: SQL"%" + C# parameter + SQL"% 
            cmd.Prepare();


            //Gather REsult Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Students List 
            List<Student> Students = new List<Student>();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

               
    
                Student NewStudent = new Student();

                //new student has properties associated to it from database
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                int StudentID = Convert.ToInt32(ResultSet["studentid"]); //keep as convert.ToInt32!!!!

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
            MySqlConnection Conn = School.AccessDatabase();

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
