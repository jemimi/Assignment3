using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//CONNECTION TO DATABASE IN PHPMYSQL TEACHERS DATABASE ============

//Make sure to install MYSQL.Data
//INstall via Tools >NuGet package > manage nuget package for solution
//"Browse" tab
//Search for mysql.data and install to project
using MySql.Data.MySqlClient; //allows access to the tools installed 




namespace SchoolProject.Models
{
    public class SchoolDbContext
    {

        //Describes the database connection 
        //These are readonly "secret" properties
        //Only the SchoolDbContext class can use them
        //Change these to match your own local blog database!

        //private static = scope of the variable - ex. user only belongs to the class
        //static = cannot change- stays the same while the database is running
        //property accessors = controls the access of information {get{return " root";}}
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "teachers"; } }  //name of the database in PHPmyadmin 
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } } //port number from MAMP front page 

        //ConnectionString is a series of credentials used to connect to the database
        protected static string ConnectionString //compiles the string together ; private and read only 
        {
            get
            {

                return "server =" + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True"; // for teacher hiredate
                //convert zero datetime is a db connection setting which returns NULL if the date is 0000-00-00
                //this can allow C# to have an easier interpretation of the date (no date instead of 0 BCE)
            }
        }

        //This is the method we use to get the database
        ///<summary>
        /// Returns a connection to the blog database
        /// </summary>
        /// <example>
        /// private SchoolDbContext List = new SchoolDbContext();
        /// MySqlConnection Conn = List.AccessDatabase();
        /// </example>
        /// List is an object
        /// <returns> A MySqlConnection Object</returns>

        public static MySqlConnection AccessDatabase()
        {
            //We are instantiating the MySqlConnection Class to create an object
            //the object is a specific connection to our blog database on part 3306 of localhost

            //access the database
            return new MySqlConnection(ConnectionString); //connection string is defined above
        }
    }
}