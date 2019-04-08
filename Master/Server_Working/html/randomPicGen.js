window.onload = choosePic;

var myPix = new Array("images/LoginMemes/1.jpg","images/LoginMemes/2.jpg","images/LoginMemes/3.jpg");

function choosePic() {
	randomNum = Math.floor((Math.random() * myPix.length));
	document.getElementById("myPicture").src = myPix[randomNum];
}
