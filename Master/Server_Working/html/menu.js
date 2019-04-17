function MenuUpload() {
	var x = document.getElementById("UploadB");
  
	var req = new XMLHttpRequest();
	req.onload = function() {
		
		if (req.response == "-1") {
			x.style.display = "block";
		} else {
			x.style.display = "none";
		}
	}
	
	req.open("GET", "getSessionUid", false);
	req.send();
}
