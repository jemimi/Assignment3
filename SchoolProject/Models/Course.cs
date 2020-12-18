using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Course
    {
        //primary key
        //The following fields (not properties) define a Course (i.e. class)
        public int CourseId;
        public string CourseName;
        public string CourseCode;
        public DateTime StartDate;
        public DateTime EndDate; 

        //Foreign Key
        public int TeacherId;

        //can Execute Server Validation Logic here
        //see Teacher.cs as an example
        public bool IsValid()
        {
            return true;
        }

        //parameter-less constructor function
        //used for auto-binding Course properties in ajax call to CourseData Controller

        public Course() { }
       
        
    }
}