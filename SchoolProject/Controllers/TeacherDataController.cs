using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models; // new reference to projects in the model folder
using MySql.Data.MySqlClient; //reference to MySQL database
using System.Diagnostics;
using System.Web.Http.Cors; //for AJAX javascript in order to add new teacher via form; need to install Microsoft.ASPnet.cors or Microsoft.ASPnet.webapi.cors

namespace SchoolProject.Controllers
{
    public class TeacherDataController : ApiController
    //type of controller
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext(); //1. new instance of a class as teacher object

        //This Controller will access the teachers table of our teacher database
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>A list of teachers (first names and last names)
        /// </returns>

        [HttpGet]
        //configure the route attribute for the searchkey for form
        //? means that the information may or may not be included
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null) //2. method  = List Teachers as a string
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //OPen the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database 
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";
            //lower means lowercase
            //commandtext is public This is s command object. represents a string

            //security: anything that is included - the @ key is the search key
            //don't have to worry about tampering with SQL injection attacks. 
            //Any strange characters will get stripped out
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set 


            //Create an empty list of Teachers
            //what it does:  finds the teacher names and adds them to a new list
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set 
            //read method will proceed through list via rows. result set will return a dual data type
            // 1 loop will result in one result set. ex. if 300 teachers - will loop 300 times
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]); //keep as Convert.ToInt32; int does not work
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                
                //Access the Teacher hiredate - safter way by parsing the result into a string 
               // DateTime TeacherHireDate;
                //DateTime.TryParse(ResultSet["hiredate"].ToString(), out TeacherHireDate);


                //Create a new teacher object
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId; //TeacherId on left refers to teacher.cs class from Models folder
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                //NewTeacher.TeacherHireDate = TeacherHireDate;


                //Add the Teacher  to the List of teachers
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers 
            return Teachers;
        }

        /// <summary>
        /// Finds a teacher from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <returns>Teacher object containing information about the teacher with a matching ID. Empty Teacher Object if the ID does not match any teachers in the system.</returns>
        /// <example>api/TeachrData/FindTeacher/6 -> {Teacher Object}</example>
        /// <example>api/TeachrData/FindTeacher/10 -> {Teacher Object}</example>

        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //OPen the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database 
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from teachers where teacherid =  @id"; //commandtext is public This is s command object. represents a string
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set 

            //loops through the database depending on the number of rows
            while (ResultSet.Read())
            {
                //Access Column info by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet ["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();

                //access the teacher hire date -safer way by parsing the result into a string
                // DateTime TeacherHireDate;
                //DateTime.TryParse(ResultSet["hiredate"].ToString(), out TeacherHireDate);

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                //NewTeacher.TeacherHireDate = TeacherHireDate;
            }

            Conn.Close();

            return NewTeacher;
        }


        /// <summary>
        /// Deletes an Teacher from the connected MySQL Database if the ID of that Teacher exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">Teacher Id.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>


        //need to specify a return type: void. This means that nothing needs to be returned
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "DELETE from teachers where teacherid = @id";

            cmd.Parameters.AddWithValue("@id", id);
          
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }


        /// <summary>
        /// Adds a new teacher to the MySQL database. Non-deterministic.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// FORM DATA / POST DATA / REQUEST 
        /// 
        ///{
        ///     "TeacherFname: "Martha",
        ///     "TeacherLname: "Lee",
        ///  
        /// }
        /// </example>


        // CORs connection needed for AJAX javascript 
        // must download and install Cors

        [HttpPost]
        [EnableCors(origins: "*", methods:"*", headers:"*" )]
         // Input an teacher object
         public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //Create an instance of a connectoin
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            //Open the connection between server and database
            Conn.Open();

            //Create a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY: 
            //query to the database using SQL query language
            cmd.CommandText = "INSERT INTO Teachers (TeacherFname, TeacherLname, hiredate) values (@TeacherFname, @TeacherLname, @hiredate)";
            //Set the parameters
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            //cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Hiredate", NewTeacher.HireDate);
            //cmd.Parameters.AddWithValue("@salary", NewTeacher.salary);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            // get the last inserted id when showing the Teacher immediately after creating it 

            Conn.Close();

         }


    /// <summary>
    /// Updates an Teacher on the MySQL Database. Non-Deterministic.
    /// </summary>
    /// <param name="TeacherInfo">An object with fields that map to the columns of the teachers's table.</param>
    /// <example>
    /// POST api/TeacherData/UpdateTeacher/208 
    /// FORM DATA / POST DATA / REQUEST BODY 
    /// {
    ///
    ///     "TeacherFname: "Martha",
    ///     "TeacherLname: "Lee",
    ///  
    /// }
    /// </example>
    public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(TeacherInfo.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "UPDATE teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname  where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }


    }
}
