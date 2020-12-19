using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models.ViewModels
{
    public class ShowTeacher
    {

        //NOTE: create a new model to add teachersxclasses 
        //public List< Course> ClassesTaught;  

        //The following fields (not properties) define a Course (i.e. class)
        public Teacher Teacher;

        //Courses Taught:
        public IEnumerable<Course>CoursesTaught;

        //Students
        public IEnumerable<Student> StudentsTaught;


    }
    
}