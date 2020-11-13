using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models; // new reference to projects in the model folder
using MySql.Data.MySqlClient; //reference to MySQL database

namespace SchoolProject.Controllers
{
    public class TeacherDataController : ApiController
    //type of controller
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext Teachers = new SchoolDbContext(); //1. new instance of a class as teacher object

        //This Controller will access the teachers table of our teacher database
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>A list of teachers (first names and last names)
        /// </returns>

        [HttpGet]


        public IEnumerable<Teacher> ListTeachers()
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //OPen the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database 
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from Teachers"; //commandtext is public This is s command object. represents a string

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set 


            //Create an empty list of Teachers
            //what it does:  finds the teacher names and adds them to a new list
            List<Teacher> Teacher = new List<Teacher> { };

            //Loop Through Each Row the Result Set 
            //read method will proceed through list via rows. result set will return a dual data type
            // 1 loop will result in one result set. ex. if 300 teachers - will loop 300 times
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];


                //Create a new teacher object
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId; //TeacherId on left refers to teacher.cs class from Models folder
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;


                //Add the Teacher  to the List of teachers
                Teacher.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers 
            return Teacher;
        }



        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //OPen the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database 
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from teachers where teacherid = "+id; //commandtext is public This is s command object. represents a string
            
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set 

            //loops through the database depending on the number of rows
            while (ResultSet.Read())
            {
                //Access Column info by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                //string HireDate = (string)ResultSet["hiredate"];

                NewTeacher.TeacherId = TeacherId; //TeacherId on left refers to teacher.cs class
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                //NewTeacher.HireDate = HireDate;


            }


            return NewTeacher;
        }

        //======= SETUP to display courses taught by a teacher ===============
        //[HttpGet]
        //public WebClass FindClass()
        //{
        //WebClass NewWebClass = new WebClass();

        //Create an instance of a connection
        //MySqlConnection Conn = Teachers.AccessDatabase();

        //OPen the conn or connection between the web server and database
        //Conn.Open();

        //Establish a new command (query) for our database 
        // MySqlCommand cmd = Conn.CreateCommand();

        //SQL Query
        //cmd.CommandText = "SELECT classname From teachers left join classes on teachers.teacherid=classes.teacherid where teacherid =" 

        //cmd.CommantText = "SELECT classname, teachers.teacherid, classes.teacherid, CONCAT(teacherfname, " ", teacherlname) FROM `teachers` LEFT JOIN `classes` ON teachers.teacherid=classes.teacherid WHERE teacherid="+id;

        //Gather Result Set of Query into a variable
        //MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set 

        //loops through the database depending on the number of rows
        // while (ResultSet.Read())
        //{
        //Access Column info by the DB column name as an index
        // int TeacherId = (int)ResultSet["teacher.teacherid"];
        // string WebClassName = ResultSet["classname"].ToString();

        //string HireDate = (string)ResultSet["hiredate"];

        //  NewWebClass.TeacherId = TeacherId; //TeacherId on left refers to teacher.cs class
        // NewWebClass.WebClass= NewWebClass;




        // }


        //return NewWebClass;
    }



}
