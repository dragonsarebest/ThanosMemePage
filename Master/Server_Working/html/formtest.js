"use strict";

function sendRequest(url, formdata, callback, type){
	console.log("Sending Request");
    var req = new XMLHttpRequest();
	req.onload = function() {
		console.log(url+": "+req.response);
		if(callback != undefined)
		{callback(req.response);}
	}
    req.open(type, url, true);
    req.send( formdata );
}

function addRecord(){
	var inp1 = document.getElementById("registerPassword");
	var inp2 = document.getElementById("confirmedPassword");
	var inp3 = document.getElementById("name");
	if(inp1.value === inp2.value)
	{
		if(inp1.value.length >= 8)
		{
			if(inp1.value.length <= 20)
			{
				if(inp3.value.length <= 16)
				{
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
					document.getElementById("nameMessage").innerHTML = "Maximum Username Length = 16 Characters"
			}
			else
			{
				if(inp3.value.length > 16)
					document.getElementById("nameMessage").innerHTML = "Maximum Username Length = 16 Characters"
				document.getElementById("messages").innerHTML = "Maximum Password Length = 20 Characters"
			}
		}
		else
		{
			if(inp3.value.length > 16)
				document.getElementById("nameMessage").innerHTML = "Maximum Username Length = 16 Characters"
			document.getElementById("messages").innerHTML = "Minimum Password Length = 8 Characters"
		}
			
		
	}
	else
	{
		
		inp1.style.background = "#ff0000";
		inp2.style.background = "#ff0000";
		alert("Passwords Do Not Match");
		console.log("You're passwords don't match!");
	}
}

function removeUserLength(){
	var inp1 = document.getElementById("name");
	if(inp1.value.length <= 16)
		document.getElementById("nameMessage").innerHTML = ""
}

function unHighlight(){
	var inp1 = document.getElementById("registerPassword");
	var inp2 = document.getElementById("confirmedPassword");
	if(inp1.value.length > 8 && inp1.value.length < 20)
		document.getElementById("messages").innerHTML = ""
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

function checkPassword(){
	var pass = document.getElementById("registerPassword").value;
	var strengthBar = document.getElementById("strength");
	var strength = 0;
	
	if(pass.match(/[]+/))
		strength += 1;
	if(pass.match(/[a-zA-Z0-9]+/))
		strength += 1;
	if(pass.match(/[~<>?!]+/))
		strength += 1;
	if(pass.match(/[!@#$%^&*()]+/))
		strength += 1;
	if(pass.length > 8)
		strength += 1;
	
	switch(strength)
	{
		case 0:
			strengthBar.value = 0;
			break;
			
		case 1:
			strengthBar.value = 15;
			strengthBar.style.background = "red"
			break;
			
		case 1:
			strengthBar.value = 30;
			break;
			
		case 2:
			strengthBar.value = 45;
			strengthBar.style.background = "Blue"
			break;
			
		case 3:
			strengthBar.value = 70;
			break;
			
		case 4:
			strengthBar.value = 85;
			strengthBar.style.background = "Green"
			break;
			
		case 5:
			strengthBar.value = 100;
			break;
	}
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
			window.location.href = "index.html";
		}
		else
		{
			inp1.style.background = "#ff0000";
			inp2.style.background = "#ff0000";
			alert("Login Failed");
			console.log("Login Failed");
		}
	}
    req.open("GET", "getSessionUid", true);
    req.send();
	
}