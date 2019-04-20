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

function logout() {
    var fd = new FormData();
	sendRequest("doLogout", "POST");
	console.log("Log out sucessful");
	window.location.href = "home.html";
}

function display() {
		var element = document.getElementById("AccountB");
		var text = getUsername();
		element.appendChild(text);
}
