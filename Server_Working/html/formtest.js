"use strict";

function addRecord(){
    var inp = document.getElementById("value");    
    var fd = new FormData();
    fd.append( "username", document.getElementById("name").value );
	fd.append( "email", document.getElementById("email").value );
	fd.append( "password", document.getElementById("password").value );
    var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log("addRecord: "+req.responseText);
            if( callback != undefined )
                callback(req);
        }
    });
    req.open("POST", "addRecord" );
    req.send( fd );
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