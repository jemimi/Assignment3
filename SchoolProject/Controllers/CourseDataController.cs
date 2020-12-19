using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class CourseDataController : ApiController
    {

        // The database context class which allows us to access our MySQL Database.
        //private SchoolDbContext School = new SchoolDbContext(); //1. new instance of a class as blog object

        //Create an instance of a connection
        MySqlConnection Conn = SchoolDbContext.AccessDatabase();


        //This Controller will access the classes table of our blog database
        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>GET api/CourseData/ListSttudents</example>
        /// <returns>A list of Classes</returns>
        /// 
        /// 
        /// 

        [HttpGet]
        [Route("api/CourseData/ListCourses/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Course> ListCourses(string SearchKey = null)
        {
            

            //Create an empty list of Courses
            List<Course> Courses = new List<Course> { };

            try
            {


                //Open the conn or connection between the web server and database
                Conn.Open();

                //Establish a new command(query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL Query
                cmd.CommandText = "SELECT * from Classes where lower(classname) like lower(@key) or lower(classcode) like lower(@key)"; //accesses info from classes table

                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather REsult Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set



                //Loop through each row the result set
                while (ResultSet.Read())
                {
                    //Access Column info by DB column name as an index


                    int CourseId = Convert.ToInt32(ResultSet["classid"]);
                    string CourseName = ResultSet["classname"].ToString();
                    string CourseCode = ResultSet["classcode"].ToString();

                    //Start and End dates
                    DateTime StartDate = (DateTime)ResultSet["startdate"];
                    DateTime EndDate = (DateTime)ResultSet["finishdate"];


                    //NewCourse definitions
                    Course NewCourse = new Course();

                    NewCourse.CourseName = CourseName;
                    NewCourse.CourseCode = CourseCode;
                    NewCourse.CourseId = CourseId;
                    NewCourse.StartDate = StartDate;
                    NewCourse.EndDate = EndDate;

                    //Class objects are added to NewCourse
                    Courses.Add(NewCourse);
                }

            }

            catch (MySqlException ex)
            {
                //Cathces issues with MySQL
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connetion between MySQL Database and the WebServer
                Conn.Close();
            }

            //Return final list of Classes
            return Courses;

        }

        /// <summary>
        /// Returns a list of Courses of a teacher
        /// </summary>
        /// <param name="Teacherid">course primary key</param>
        /// <returns></returns>

        //FOR THE VIEW MODEL = COURSES AND TEACHERS ON SAME VIEW

        [HttpGet]
        [Route("api/CourseData/ListCourses/{Teacherid}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Course> GetCoursesForTeacher(int TeacherId)
        {

            
            //Create an empty list of Courses
            List<Course> Courses = new List<Course> { };
            try
            {
                //Try to open connection between server and database
                Conn.Open();

                //Establish new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Classes where classes.teacherid=@TeacherId";

                cmd.Parameters.AddWithValue("@TeacherId", TeacherId);
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int CourseId = Convert.ToInt32(ResultSet["classid"]);
                    string CourseName = ResultSet["classname"].ToString();
                    string CourseCode = ResultSet["classcode"].ToString();
                    DateTime StartDate = (DateTime)ResultSet["startdate"];
                    DateTime EndDate = (DateTime)ResultSet["finishdate"];


                    Course NewCourse = new Course();
                    NewCourse.CourseId = CourseId;
                    NewCourse.CourseName = CourseName;
                    NewCourse.CourseCode = CourseCode;
                    NewCourse.StartDate = StartDate;
                    NewCourse.EndDate = EndDate;

                    //Add Course name to list
                    Courses.Add(NewCourse);

                }
            }

            catch (MySqlException ex)
            {
                //Catches issues with MySQL
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("Server issue", ex);
            }
            finally
            {
                //Close the connection between MYSQL Database and the WebServer
                Conn.Close();
            }

            //Return the final list of Course names
            return Courses;

        }


        /// <summary>
        /// Finds a Course from the MySQL Database via ID. Non-deterministic 
        /// </summary>
        /// <param name="id">CourseId</param>
        /// <returns>
        /// Course object containing information about Course with a matching ID. 
        /// Empty Course Object if no matching course in system.
        /// </returns>
        /// <example>api/CourseData/FindCourse/6 --> {Course Object}</example>

        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public Course FindCourse(int id) //Findcourse links with the CourseController.cs 
        {

            

            Course NewCourse = new Course();

            try
            {


                //Open the conn or connection between the web server and database
                Conn.Open();

                //Establish a new command(query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL Query
                cmd.CommandText = "SELECT * from Classes WHERE classid= @id"; //accesses info from classes table
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

                //Loop through each row the result set
                while (ResultSet.Read())
                {

                    string CourseName = ResultSet["classname"].ToString();
                    string CourseCode = ResultSet["classcode"].ToString();
                    int CourseId = Convert.ToInt32(ResultSet["classid"]);
                    DateTime StartDate = (DateTime)ResultSet["startdate"];
                    DateTime EndDate = (DateTime)ResultSet["finishdate"];

                    //Create an empty list of Classes List 
                    //Course NewCourse = new Course();
                    //NewCourse definitions
                    NewCourse.CourseId = CourseId;
                    NewCourse.CourseName = CourseName;
                    NewCourse.CourseCode = CourseCode;
                    NewCourse.StartDate = StartDate;
                    NewCourse.EndDate = EndDate;

                }
                //checking model validity after pulling from DB
                if (!NewCourse.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);

            }

            //catching an error from server side 
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That Course was not found.", ex);
            }
            //catching error from SQL
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("The issue is caused by database", ex);
            }

            //Catches other issues: 
            catch (Exception ex)
            {
                //write() will output but does not break to next line but writeline() does
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close connection between MySQL and Server
                Conn.Close();
            }


            return NewCourse;


        }

        /// <summary>
        ///  Deletes an Course from the connected MySQL Database if the ID of that Course exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">Course Id</param>
        /// <example>POST /api/CourseData/DeleteCourse/3</example>

        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteCourse(int id)
        {
            

            try
            {
                //open connection between server and DB
                Conn.Open();

                //Establish a new command (query) for database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL query
                cmd.CommandText = "DELETE from Classes where classid=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //EXECUTENONQUERY: 
                //used to execute SQL Command or the storeprocedure performs, 
                //INSERT, UPDATE or Delete operations. 
                //It doesn't return any data from the database. 
                //Instead, it returns an integer specifying the number of rows 
                //inserted, updated or deleted.
                cmd.ExecuteNonQuery();
            }

            catch (MySqlException ex)
            {
                //Catches issues with SQL
                Debug.WriteLine(ex);
                throw new ApplicationException("Database issue", ex);
            }
            catch (Exception ex)
            {
                //Catches other issues
                Debug.Write(ex);
                throw new ApplicationException("Server issue", ex);
            }
            finally
            {
                //Close connection between SQL DB and server
                Conn.Close();
            }


        }

        // <summary>
        /// Adds an Course to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewCourse">An object with fields that map to the columns of the Course's table. </param>
        /// <example>
        /// POST api/CourseData/AddCourse 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"CourseName":"Web Application Development ",
        ///	"CourseCode":"http5101",
        /// }
        /// </example>

        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddCourse([FromBody] Course NewCourse)
        {

            

            //Exit method if model fields are not incuded
            if (!NewCourse.IsValid()) throw new ApplicationException("Posted Data not valid");

            try
            {
                //Open connection between server and DB
                Conn.Open();

                //Establish new command (query) for DB
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL query
                cmd.CommandText = "INSERT into Classes (classcode, startdate, enddate, classname ) values (@classcode, @startdate, @enddate,@classname  )";
                
                cmd.Parameters.AddWithValue("@classcode", NewCourse.CourseCode);
                cmd.Parameters.AddWithValue("@startdate", NewCourse.StartDate);
                cmd.Parameters.AddWithValue("@enddate", NewCourse.EndDate);
                cmd.Parameters.AddWithValue("@classname", NewCourse.CourseName);

                cmd.Prepare();

                cmd.ExecuteNonQuery();

              

            }
            catch (MySqlException ex)
            {
                //Catches issues with SQL
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue with Database", ex);
            }

            catch (Exception ex)
            {
                //Catches other issues
                Debug.WriteLine(ex);
                throw new ApplicationException("Server issue", ex);
            }

            finally
            {
                //close connection between SQL and server:
                Conn.Close();
            }
        }

        /// <summary>
        /// Updates an Course on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="CourseInfo">An object with fields that map to the columns of the Classes's table.</param>
        /// <example>
        /// POST api/CourseData/UpdateCourse/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"CourseTitle":"My Sound Adventure in Italy",
        ///	"CourseBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateCourse(int id, [FromBody] Course CourseInfo)
        {
            

            //Exit method if model fields are not included.
            if (!CourseInfo.IsValid()) throw new ApplicationException("Posted Data was not valid.");

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Classes SET classcode =@classcode, startdate = @startdate, finishdate=@finishdate, classname =@classname WHERE classid=@classId";
                cmd.Parameters.AddWithValue("@Classcode", CourseInfo.CourseCode);
                cmd.Parameters.AddWithValue("@CourseName", CourseInfo.CourseName);
                cmd.Parameters.AddWithValue("@Startdate", CourseInfo.StartDate);
                cmd.Parameters.AddWithValue("@Enddate", CourseInfo.EndDate);
                cmd.Parameters.AddWithValue("@CourseId", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }

        }

    }

}
