"use strict";

function addRecord(){
	var inp1 = document.getElementById("password");
	var inp2 = document.getElementById("confirmedpassword");
	if(inp1.value === inp2.value){
		var inp = document.getElementById("value");    
		var fd = new FormData();
		fd.append( "username", document.getElementById("name").value );
		fd.append( "email", document.getElementById("email").value );
		fd.append( "password", document.getElementById("password").value );
		var req = new XMLHttpRequest();
		req.addEventListener( "load", () => {
			if( req.readyState === 4 && req.status === 200 ){
				console.log("addRecord: "+req.responseText);
				//window.location.replace("http://localhost:8888/login.html")
				if( callback != undefined )
					callback(req);
			}
		});
		req.open("POST", "addRecord" );
		req.send( fd );
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

function doLogin(){
	var inp = document.getElementById("signup-form");    
    var fd = new FormData();
	fd.append( "email", document.getElementById("email").value );
	fd.append( "password", document.getElementById("password").value );
	var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log("doLogin: "+req.responseText);
            if( callback != undefined )
                callback(req);
        }
    });
    req.open("POST", "doLogin" );
    req.send( fd );
	alert("Success!");
}