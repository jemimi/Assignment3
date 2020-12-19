using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;

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

        public bool IsValid()
        {
            bool valid = true;

            if (TeacherFname == null || TeacherLname == null )
            {
                //Base validation to check if the fields are entered.
                valid = false;
            }
            else
            {
                //Validation for fields to make sure they meet server constraints
                if (TeacherFname.Length < 2 || TeacherFname.Length > 255) valid = false;
                if (TeacherLname.Length < 2 || TeacherFname.Length > 255) valid = false;
            
            }
            Debug.WriteLine("The model validity is : " + valid);

            return valid;
        }

        //Parameter-less constructor function
        //Necessary for AJAX requests to automatically bind from the [FromBody] attribute
        public Teacher() { }
        // NOTE: create a new model to add teachersxclasses 
        //public List< Course> ClassesTaught;  

        //parameter-less constructor function
       
    }
}