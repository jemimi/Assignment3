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
    public class StudentDataController : ApiController
    {

        // The database context class which allows us to access our MySQL Database
        //AccessDatabase switched to a static method, one that can be called without an object.
        //Just need to write it one time: 
        MySqlConnection Conn = SchoolDbContext.AccessDatabase(); //1. new instance of a class as blog object


        //This Controller will access the students table of our blog database
        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>GET api/StudentData/ListSttudents</example>
        /// <returns>A list of students</returns>
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
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Student> ListStudents(string SearchKey = null) //Student is a class from the models.student.cs
        {

            //Create empty list of Students: 
            List<Student> Students = new List<Student> { };

            try
            {
                //Open the conn or connection between the web server and database
                Conn.Open();

                //Establish a new command(query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL Query
                //accesses info from SQL students table
                cmd.CommandText = "select * from students where studentfname like @key or studentlname like @key";


                cmd.Parameters.AddWithValue("@Key", "%" + SearchKey + "%"); // partial query: SQL"%" + C# parameter + SQL"% 
                cmd.Prepare();


                //Gather REsult Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set



                //Loop through each row the result set
                while (ResultSet.Read())
                {



                    Student StudentInfo = new Student();

                    //new student has properties associated to it from database
                    string StudentFname = ResultSet["studentfname"].ToString();
                    string StudentLname = ResultSet["studentlname"].ToString();
                    int StudentID = Convert.ToInt32(ResultSet["studentid"]); //keep as convert.ToInt32!!!!

                    //StudentInfo definitions
                    StudentInfo.StudentFname = StudentFname;
                    StudentInfo.StudentLname = StudentLname;
                    StudentInfo.StudentId = StudentID;

                    //Student objects are added to StudentInfo
                    Students.Add(StudentInfo);
                }

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


                //Close the connetion between MySQL Database and the WebServer
                Conn.Close();


            }

            //Return final list of student
            return Students;

        }


        /// <summary>
        /// Finds an Student from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Student ID</param>
        /// <returns>Student object containing information about the Student with a matching ID. Empty Student Object if the ID does not match any Students in the system.</returns>
        /// <example>api/StudentData/FindStudent/6 -> {Student Object}</example>
        /// <example>api/StudentData/FindStudent/10 -> {Student Object}</example>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{TeacherId}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Student FindStudent (int id)
        {
            //Create an empty list of Students
            Student NewStudent = new Student();
            try
            {
                //Try to open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Students where Studentid = @id";

                cmd.Parameters.AddWithValue("@id", @id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int StudentId = Convert.ToInt32(ResultSet["Studentid"]);
                    string StudentFname = ResultSet["StudentFname"].ToString();
                    string StudentLname = ResultSet["StudentLname"].ToString();
                    //DateTime EnrolDate = (DateTime)ResultSet["enroldate"];
                    string StudentNumber = ResultSet["StudentNumber"].ToString();


                   
                    NewStudent.StudentId = StudentId;
                    NewStudent.StudentFname = StudentFname;
                    NewStudent.StudentLname = StudentLname;
                    //NewStudent.EnrolDate= EnrolDate;

                 
                }

                // checking for model validity after pulling from the db
                if (!NewStudent.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That Student was not found.", ex);
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

            //Return the final list of Student names
            return NewStudent;


        }





        /// <summary>
        /// Updates an Student on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="StudentInfo">An object with fields that map to the columns of the Student's table.</param>
        /// <example>
        /// POST api/StudentData/UpdateStudent/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"Student Name "Jane Lee",
        ///	       
        /// </example>

        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateStudent (int id, [FromBody]Student StudentInfo) //not expecting to return anything, use "void" 
        {
            //Exit method if model fields are not included.
            if (!StudentInfo.IsValid()) throw new ApplicationException("Posted Data was not valid.");

            try
            {
                //open connection between server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL Query
                cmd.CommandText = "UPDATE students SET (studentfname= @StudentFname, studentlname=@StudentLname) where studentid=@studentId";
                //need a where studentId so that a specific student is updated and not all students
                cmd.Parameters.AddWithValue("@StudentFname", StudentInfo.StudentFname);
                cmd.Parameters.AddWithValue("@StudentLname", StudentInfo.StudentLname);
                cmd.Parameters.AddWithValue("@StudentId", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery(); //only 1 row should be affected by this update statement
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
