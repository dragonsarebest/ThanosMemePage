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
	var inp1 = document.getElementById("password");
	var inp2 = document.getElementById("confirmedpassword");
	if(inp1.value === inp2.value){
		var inp = document.getElementById("value");    
		var fd = new FormData();
		fd.append( "username", document.getElementById("name").value );
		fd.append( "email", document.getElementById("email").value );
		fd.append( "password", document.getElementById("password").value );
		sendRequest("addRecord", fd, undefined, "POST");
		
	}
	else
	{
		inp1.style.background = "#ff0000";
		inp2.style.background = "#ff0000";
		setTimeout(()=>{
			inp1.style.background = "white";
			inp2.style.background = "white";
			}, 750);
		alert("Passwords don't match!");
		console.log("You're passwords don't match!");
	}
}

function updatePage(Uid)
{
	// The current issue is that no matter how you pass the xmlhttp.response (the Uid in this case) it becomes
	// undefined in the local function
	console.log("///TESTING///Current Uid = " + Uid);
	if(Uid != "-1")
	{
		alert("Successful Login");
		console.log("Successful Login");
		window.location.href = "Home.html";
	}
	else
	{
		alert("Failed Login");
		console.log("Failed Login");
		window.location.href = "userFail.html";
	}
}

function doLogin(){
	var inp = document.getElementById("signup-form");    
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
			alert("Successful Login");
			console.log("Successful Login");
			window.location.href = "Home.html";
		}
		else
		{
			alert("Failed Login");
			console.log("Failed Login");
			window.location.href = "userFail.html";
		}
	}
    req.open("GET", "getSessionUid", false);
    req.send();
	
}