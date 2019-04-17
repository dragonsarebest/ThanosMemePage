"use strict";

function sendRequest(url, formdata, callback, type){
	console.log("Sending Request");
    var req = new XMLHttpRequest();
	req.onload = function() {
		console.log(url+": "+req.response);
		if(callback != undefined)
		{callback(req.response);}
	}
    req.open(type, url, false);
    req.send( formdata );
}

function addRecord(){
	var inp1 = document.getElementById("registerPassword");
	var inp2 = document.getElementById("confirmedPassword");
	if(inp1.value === inp2.value){
		var inp = document.getElementById("value");    
		var fd = new FormData();
		fd.append( "username", document.getElementById("name").value );
		fd.append( "email", document.getElementById("registerEmail").value );
		fd.append( "password", document.getElementById("registerPassword").value );
		alert("Account Successfully Created");
		sendRequest("addRecord", fd, undefined, "POST");
		window.location.href = "User.html";
		
	}
	else
	{
		inp1.style.background = "#ff0000";
		inp2.style.background = "#ff0000";
		alert("Passwords Do Not Match");
		console.log("You're passwords don't match!");
	}
}

function unHighlight(){
	var inp1 = document.getElementById("registerPassword");
	var inp2 = document.getElementById("confirmedPassword");
	inp1.style.background = "white";
	inp2.style.background = "white";
	console.log("Changing Background");
}

function loginUnHighlight(){
	var inp1 = document.getElementById("email");
	var inp2 = document.getElementById("password");
	inp1.style.background = "white";
	inp2.style.background = "white";
	console.log("Changing Background");
}

function doLogin(){
	var inp = document.getElementById("signup-form"); 
	var inp1 = document.getElementById("email");
	var inp2 = document.getElementById("password");	
    var fd = new FormData();
	fd.append( "email", document.getElementById("email").value );
	fd.append( "password", document.getElementById("password").value );
	sendRequest("doLogin", fd, undefined, "POST");
	
	console.log("Sending Request");
    var req = new XMLHttpRequest();
	req.onload = function() {
		console.log("getSessionUid: "+req.response);
		if(req.response != "-1")
		{
			alert("Login Successful");
			console.log("Login Successful");
			window.location.href = "home.html";
		}
		else
		{
			inp1.style.background = "#ff0000";
			inp2.style.background = "#ff0000";
			alert("Login Failed");
			console.log("Login Failed");
		}
	}
    req.open("GET", "getSessionUid", false);
    req.send();
	
}