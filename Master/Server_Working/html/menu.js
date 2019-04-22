function sendRequest(url, formdata, callback){
    var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log(url+": "+req.responseText);
            if( callback != undefined )
                callback(req);
        }
    });
    req.open("POST", url );
    req.send( formdata );
}

function MenuUpload() {
	var x = document.getElementById("UploadB");
	var y = document.getElementById("AccountB");
	var z = document.getElementById("RegisterB");
	
	var req = new XMLHttpRequest();
	req.onload = function() {
		
		if (req.response == "-1") {
			x.style.display = "none";
			y.style.display = "none";
			z.style.display = "block";
		} else {
			x.style.display = "block";
			y.style.display = "block";
			z.style.display = "none";
		}
	}
	
	req.open("GET", "getSessionUid", false);
	req.send();
}

//returns the username from current session
function getUsername() {
	var text = document.createTextNode('testing');
	var req = new XMLHttpRequest();
	req.onload = function() {
		
		if (req.response == "not logged in") {
			text = document.createTextNode('Username');
		} else {
			text = document.createTextNode(req.response);
		}
	}
	req.open("GET", "getSessionUsername", false);
	req.send();
	return text;
}

//logs out of current session
function logout() {
    var fd = new FormData();
	sendRequest("doLogout", "POST");
	console.log("Log out sucessful");
	window.location.href = "index.html";
}

function display() {
		var element = document.getElementById("AccountB");
		var text = getUsername();
		element.appendChild(text);
}

function displayEmail() {
		var text = 'testing';
		var req = new XMLHttpRequest();
		req.onload = function() {
			
			if (req.response == "not logged in") {
				text = 'Email';
			} else {
				text = req.response;
			}
		}
		req.open("GET", "getSessionEmail", false);
		req.send();
		return text;
}

function displayUsername() {
		var text = 'testing';
		var req = new XMLHttpRequest();
		req.onload = function() {
			
			if (req.response == "not logged in") {
				text = 'username';
			} else {
				text = req.response;
			}
		}
		req.open("GET", "getSessionUsername", false);
		req.send();
		return text;
}

function updateInformation(){	
	var username = document.getElementById("Username"); 
	var email = document.getElementById("Email");
	var oldpassword = document.getElementById("oldPassword");	
	var confoldpassword = document.getElementById("confirmedOldPassword");	
	var newpassword = document.getElementById("newPassword");	
    var fd = new FormData();
	fd.append( "email", document.getElementById("Email").value );
	fd.append( "username", document.getElementById("Username").value );
	fd.append( "password", document.getElementById("newPassword").value );
	fd.append( "oldPassword", document.getElementById("oldPassword").value );
	
    var req = new XMLHttpRequest();
	req.onload = function() {
		if(document.getElementById("oldPassword").value == document.getElementById("confirmedOldPassword").value && newpassword.value.toString().length == 0)
		{
			if(oldpassword.value.toString().length == 0)
			{
				oldpassword.style.background = "#ff0000";
				confoldpassword.style.background = "#ff0000";
				alert("You must enter your old password!");
				console.log("Update Failed");
			}
			else
			{
				alert("Account Updated!");
				fd.append( "uid", req.response );
				sendRequest("UpdateRecordNoPassword", fd, undefined, "POST");
				window.location.href = "account.html";
			}
		}
		else if(document.getElementById("oldPassword").value != document.getElementById("confirmedOldPassword").value && newpassword.value.toString().length == 0)

		{
			oldpassword.style.background = "#ff0000";
			confoldpassword.style.background = "#ff0000";
			alert("Old Passwords Must Match");
			console.log("Update Failed");
		}
		
		else if(document.getElementById("oldPassword").value == document.getElementById("confirmedOldPassword").value && newpassword.value.toString().length > 0)
		{
			alert("Account Updated!");
			fd.append( "uid", req.response );
			sendRequest("UpdateRecord", fd, undefined, "POST");
			window.location.href = "account.html";
		}
		else if(document.getElementById("oldPassword").value != document.getElementById("confirmedOldPassword").value && newpassword.value.toString().length > 0)
		{
			oldpassword.style.background = "#ff0000";
			confoldpassword.style.background = "#ff0000";
			alert("Old Passwords Must Match");
			console.log("Update Failed");
		}
	}
    req.open("GET", "getSessionUid", false);
    req.send();
	
}
