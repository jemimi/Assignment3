//AJAX for Teacher ADD
//Connected to Shared/_Layout.cshtml




function AddTeacher() {
    //goal: send a request : 
    //POST: http://localhost:62007/api/TeacherData/AddTeacher
    //with POST data of teachername

    var URL = "http://localhost:62007/api/TeacherData/AddTeacher/";

    var rq = new XMLHttpRequest();
    //need to answer 3 questions:
    //1. Request: where is the request sent to?
    //2. Method: Is this a GET or POST method? 
    //3. Response: What to do with the response?

    //getting the value from the form
    var TeacherFname = document.getElementById('TeacherFname').value;
    var TeacherLname = document.getElementById('TeacherLname').value;
    //var HireDate = document.getElementById('HireDate').value;

    //array
    var TeacherData = {
        "TeacherFname": TeacherFname,
        "TeacherLname": TeacherLname,
        //"HireDate": HireDate
        
    };

    //opening the AJAX connection: 
    rq.open("POST", URL, true);
    rq.setRequestHeader("Content-Type", "application/json");
    rq.onreadystatechange = function () {
        //ready state should be 4 AND status should be 200
       
        if (rq.readyState == 4 && rq.status == 200) {
            //request is successful and the request is finished

            //nothing to render, the method returns nothing.


        }

    }
    //POST information sent through the .send() method
    rq.send(JSON.stringify(TeacherData));

}