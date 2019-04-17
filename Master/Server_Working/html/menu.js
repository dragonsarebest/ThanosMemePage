
function MenuUpload() {
	var x = document.getElementById("UploadB");
	var y = document.getElementById("AccountB");
	var z = document.getElementById("RegisterB");
	
	var req = new XMLHttpRequest();
	req.onload = function() {
		
		if (req.response == "-1") {
			x.style.display = "block";
			y.style.display = "block";
			z.style.display = "none";
		} else {
			x.style.display = "none";
			y.style.display = "none";
			z.style.display = "block";
		}
	}
	
	req.open("GET", "getSessionUid", false);
	req.send();
}
