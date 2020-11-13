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
    public class WebClassDataController : ApiController
    {

        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext Teachers = new SchoolDbContext(); //1. new instance of a class as blog object


        //This Controller will access the classes table of our blog database
        /// <summary>
        /// Returns a list of Web DevClasses in the system
        /// </summary>
        /// <example>GET api/ArticleData/ListSttudents</example>
        /// <returns>A list of Classes</returns>
        /// 
        /// <NOTE!> Replaced the word "class" with "WebClass" </NOTE>
        /// 

        [HttpGet]
        public IEnumerable<WebClass> ListWebClasses()
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from classes"; //accesses info from classes table

            //Gather REsult Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of WebClasses List 
            List<WebClass> WebClasses = new List<WebClass>();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                WebClass NewWebClass = new WebClass();

                string WebClassName = ResultSet["classname"].ToString();
                string WebClassCode = ResultSet["classcode"].ToString();
                int WebClassID = Convert.ToInt32(ResultSet["classid"]);

                //NewClass definitions
                NewWebClass.WebClassName = WebClassName;
                NewWebClass.WebClassCode = WebClassCode;
                NewWebClass.WebClassId = WebClassID;

                //Class objects are added to NewClass
                WebClasses.Add(NewWebClass);
            }

            //Close the connetion between MySQL Database and the WebServer
            Conn.Close();

            //Return final list of Classes
            return WebClasses;
        }
       
        [HttpGet]
        [Route("api/WebClassData/FindWebClass/{webclassid}")]
        public WebClass FindWebClass(int webclassid) //Findwebclass links with the WebClassController.cs 
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the conn or connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from Classes WHERE classid=" + webclassid; ; //accesses info from classes table

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader(); //cmd.executereader is a result set

            //Create an empty list of Classes List 
            WebClass NewWebClass = new WebClass();

            //Loop through each row the result set
            while (ResultSet.Read())
            {

                string WebClassName = ResultSet["classname"].ToString();
                string WebClassCode = ResultSet["classcode"].ToString();
                int WebClassID = Convert.ToInt32(ResultSet["classid"]);

                //NewWEbClass definitions
                NewWebClass.WebClassName = WebClassName;
                NewWebClass.WebClassCode = WebClassCode;
                NewWebClass.WebClassId = WebClassID;


            }

            return NewWebClass;


        }
    }

}
