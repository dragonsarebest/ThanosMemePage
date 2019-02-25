"use strict";

function doFormSubmit(){
    var inp = document.getElementById("datepicker");
    if( inp.value === "" ){
        console.log("I guess you don't want to put in a start date");
        return;
    }
	
	var inp = document.getElementById("datepicker2");
    if( inp.value === "" ){
        console.log("I guess you don't want to put in a end date");
        return;
    }
    
    var fd = new FormData();
    fd.append( "data1data", document.getElementById("datepicker").value );
	fd.append( "data2data", document.getElementById("datepicker2").value );
    

    var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            var newdiv = document.createElement("div");
            newdiv.appendChild( document.createTextNode(req.responseText));
            var dates = document.getElementById("dates");
            dates.appendChild(newdiv);
        }
    });
    req.open("POST", "postDates" );
    req.send(fd);
}
