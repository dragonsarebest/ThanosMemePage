"use strict";

function sendRequest(url, formdata, callback, type){
	console.log("Sending Request");
    var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log(url+": "+req.responseText);
            if( callback != undefined )
                callback(req.responseText);
        }
    });
    req.open(type, url );
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
	
	sendRequest("getSessionUid", null, () => {updatePage();} , "GET");
	
}