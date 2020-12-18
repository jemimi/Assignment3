//AJAX for Teacher ADD
//Connected to Shared/_Layout.cshtml

//Timer is aded to the input window 
//Timer expires at 300ms, then search executes
//prevents a search on each key up for fast typers 

function _ListTeachers(d) {
    if (d.timer) clearTimeout(d.Timer);
    d.timer = setTimeOut(function () { ListTeachers(d.value); }, 300);
}


//LIST TEACHERS METHOD

function ListTeachers(SearchKey) {
    var URL = " http://localhost:62007/api/TeacherData/ListTeachers/" + SearchKey;

    var rq = new XMLHttpRequest();
    rq.open("GET", URL, true);
    rq.setRequestHeader("Content-Type", "application/json");
    rq.onreadystatechange = function () {
        //ready state should be 4 & status = 200
        if (rq.readyState == 4 && rq.status == 200) {
            //request = successful & finished

            var teachers = JSON.parse(rq.responseText)
        var listTeachers = document.getElementById("listteachers");
        listteachers.innerHTML = "";

        //renders content for each autor pulled from API call
            for (var i = 0; i < teacher.length; i++) {
                var row = document.createElement("div");
                row.classList = "listitem row";
                col.classList = "col-md-12";
                var link = document.createElement("a");
                link.href = "/Teacher/Show/" + teachers[i].TeacherId;
                link.innerHTML = teachers[i].TeacherFname + " " + teachers[i].TeacherLname;

                col.appendChild(link);
                row.appendChild(col);
                listteachers.appendChild(row);

            }

        }
    }

    //POST information sent through the .send() method
    rq.send();
}

// Usually Validation functions for Add and Update are separated.
// You can run into situations where information added is no longer updated, or vice versa
// However, as an example, validation is consolidated into 'ValidateTeacher'
// This is so that both Ajax and Non Ajax techniques can utilize the same client-side validation logic.
function ValidateTeacher() {

    var IsValid = true;
    var ErrorMsg = "";
    var ErrorBox = document.getElementById("ErrorBox");
    var TeacherFname = document.getElementById('TeacherFname').value;
    var TeacherLname = document.getElementById('TeacherLname').value;
    

    //First Name is two or more characters
    if (TeacherFname.length < 2) {
        IsValid = false;
        ErrorMsg += "First Name Must be 2 or more characters.<br>";
    }
    //Last Name is two or more characters
    if (TeacherLname.length < 2) {
        IsValid = false;
        ErrorMsg += "Last Name Must be 2 or more characters.<br>";
    }
    
    if (!IsValid) {
        ErrorBox.style.display = "block";
        ErrorBox.innerHTML = ErrorMsg;
    } else {
        ErrorBox.style.display = "none";
        ErrorBox.innerHTML = "";
    }


    return IsValid;
}

function AddTeacher() {

    //check for validation straight away
    var IsValid = ValidateTeacher();
    if (!IsValid) return;

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

function UpdateTeacher(TeacherId) {

    //check for validation straight away
    var IsValid = ValidateTeacher();
    if (!IsValid) return;

    //goal: send a request which looks like this:
    //POST : http://localhost:62007/api/TeacherData/UpdateTeacher/{id}
    //with POST data of Teachername, bio, email, etc.

    var URL = "http://localhost:62007/api/TeacherData/UpdateTeacher/" + TeacherId;

    var rq = new XMLHttpRequest();
    // where is this request sent to?
    // is the method GET or POST?
    // what should we do with the response?

    var TeacherFname = document.getElementById('TeacherFname').value;
    var TeacherLname = document.getElementById('TeacherLname').value;
    //var HireDate = document.getElementById('HireDate').value;
    



    var TeacherData = {
        "TeacherFname": TeacherFname,
        "TeacherLname": TeacherLname
         //"HireDate": HireDate
        
    };


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


//Helper function from : https://stackoverflow.com/questions/46155/how-to-validate-an-email-address-in-javascript
function ValidateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}