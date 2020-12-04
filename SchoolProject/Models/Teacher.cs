using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//========TABLES AND COLUMN HEADINGS FROM TEACHERS DATABASE TO C#

namespace SchoolProject.Models
{
    public class Teacher
    {

        //The following fields (not properties) define an Teacher
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string employeenumber;
        public DateTime HireDate;
        public decimal Salary;


        // NOTE: create a new model to add teachersxclasses 
        //public List< Course> ClassesTaught;  
       
        //parameter-less constructor function
        public Teacher() { }
    }
}