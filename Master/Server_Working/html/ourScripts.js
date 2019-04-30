"use strict";

//be DRY, not WET
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

function getRequest(url, callback){
    var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log(url+": "+req.responseText);
            if( callback != undefined )
                callback(req);
        }
    });
    req.open("GET", url );
	req.send();
}

var newTags = [];

var availableTags = [
  "DankMemes"
];

function addToAvailableTags(entries){
      entries.forEach(function(element) {
		if(availableTags[element] == undefined)
			availableTags.push(element);
	});
	console.log(availableTags);
};

function setUp()
{
	console.log("setup");
	var ul = document.getElementById("myList");
	var li = document.createElement("li");
	li.innerHTML = "Tags: ";
	li.style.margin = "margin:5px";
	li.style.display = "inline";
	ul.appendChild(li);
	
	getRequest("getAllTags", (r) => {
		var tagList = r.responseText.split(",");
		tagList = tagList.slice(0, tagList.length-1);
		console.log("tagList from server: " + tagList);
		addToAvailableTags(tagList);
		return;
	});	
}

// uploads image to the database and redirects user to index.html 
function subMeme(){
	var tags = subTags();
	//var ourMeme = document.getElementById("fullImage");
	var fd = new FormData();
    var fileToUpload = document.getElementById("uploadImage").files[0];
    if( fileToUpload ){
        fd.append( "meme", fileToUpload );
		fd.append( "tags", tags);
		console.log("Files[0] contained by uploadImage : " + document.getElementById("uploadImage").files[0]);
		sendRequest( "uploadMeme", fd, () => {
		window.location.replace("index.html"); //meme spread page will go here
		});
    }
	else if (localStorage.getItem("transfer_img"))
	{
		fileToUpload = localStorage.getItem("transfer_img");
		fd.append( "meme", fileToUpload );
		fd.append( "tags", tags);
		//console.log("Files[0] contained by uploadImage : " + document.getElementById("uploadImage").files[0]);
		sendRequest( "uploadMeme", fd, () => {
		window.location.replace("index.html"); //meme spread page will go here
		});
		localStorage.setItem("transfer_img", null);
	}
	else
		console.log("No files contained by uploadImage : " + document.getElementById("uploadImage").files[0]);
	
    
}

function subTags()
{
	var fd = new FormData();
	var stringTag = "";
	newTags.forEach(function(element) {
		stringTag += element + ",";
	});
    fd.append("tags", stringTag);
	sendRequest("uploadTags", fd, ()=>{;});
	return stringTag;
}

function addComment(parentElement){
	var fd = new FormData();
    var ul = document.getElementById("commentList");
    var candidate = document.getElementById("myComment");
	var req = new XMLHttpRequest();
	var uid = req.open("GET", "getSessionUid", true);
	
	if(candidate.value == "")
		return;
	
	var currentComment = document.getElementById("currentComment");
	
	var newValue = removeSpecial(candidate.value);
	newValue = removeSpaces(newValue);
	if(parentElement == undefined)
		removeComment();
	
	fd.append("comment", newValue);
	
	var li = document.createElement("li");
	li.setAttribute('id',newValue);
	var b = document.createElement('button');
	b.setAttribute("type", "button");
	b.setAttribute("class", "btn btn-info btn-lg");
	b.setAttribute("data-toggle", "modal");
	b.setAttribute("data-target", "#myModal");
	b.setAttribute("value", newValue);
	b.setAttribute("id", newValue);
	if(parentElement == undefined)
		li.setAttribute("style", "margin:5px; position: relative; ");
	else
		li.setAttribute("style", "margin:5px; position: absolute; transform: translateX(100%)");
	b.setAttribute("onClick", "setValueComment("+newValue+")");
	console.log(newValue);
	b.appendChild(document.createTextNode(candidate.value));
	
	//create a new modal for replies to this comment
	
	if(parentElement != undefined)
	{
		li.appendChild(b);
		parentElement.appendChild(li);
		fd.append("parent", parentElement.id);
		console.log(parentElement.id);
	}
	else
	{
		console.log("no parent");
		li.appendChild(b);
		ul.appendChild(li);	
		fd.append("parent", "none");
	}
	
	
	sendRequest("addComment", fd)
	
	var req = new XMLHttpRequest();
	req.addEventListener( "load", () => {
		if( req.readyState === 4 && req.status === 200 ){
			console.log("addComment: "+req.responseText);
			if( callback != undefined )
				callback(req);
		}
	});
	req.open("POST", "addComment" );
	req.send( fd );
	
	candidate.value = "";
}

function setValueComment(balue)
{
	var currentComment = document.getElementById("currentComment");
	currentComment.setAttribute("tag", balue.id);
	console.log(balue.id);
}

function replyToComment()
{
	var currentComment = document.getElementById("currentComment");
	var currentCommentID = currentComment.getAttribute("tag");
	
	var ul = document.getElementById("commentList");
		
	var list = ul.childNodes;
	
	list = ul.getElementsByTagName("*");
	
	for(var i = 0; i < list.length; i++)
	{
		var temp = list[i].id;
		if(temp != undefined)
			temp = removeSpaces(temp);
		//console.log("checking vs: " + temp);
		if(temp == currentCommentID)
		{
			var parentComment = list[i];
			addComment(list[i]);
		}
	}	
}

function removeComment()
{
	var currentComment = document.getElementById("currentComment");
	var inputBoy = currentComment.getAttribute("tag");
		
	var ul = document.getElementById("commentList");
	
	var list = ul.childNodes;
	
	list = ul.getElementsByTagName("*");
	
	for(var i = 0; i < list.length; i++)
	{
		var temp = list[i].id;
		if(temp != undefined)
			temp = removeSpaces(temp);
		//console.log("checking vs: " + temp);
		if(temp == inputBoy)
		{
			//ul.removeChild(list[i]);
			list[i].remove();
		}
	}		
}

function doRating(val)
{
	var req = new XMLHttpRequest();
	var uid = req.open("GET", "getSessionUid", true);
	if(uid != -1){
		var inp = document.getElementById("Rating_"+val);
		var fd = new FormData();
		fd.append( "Post_Id", inp.name );
		fd.append( "Rating", val );
		sendRequest("doRating", fd, undefined, "POST");
		}
}
function encodeHTML(s) {
    return s.replace('/&/g', '&amp;').replace('/</g', '&lt;').replace('/"/g', '&quot;').replace('/>/g', '&gt;');
}

function removeSpecial(s)
{
	return s.replace(/[^a-zA-Z]/g, '');
}

function strip(html){
   var doc = new DOMParser().parseFromString(html, 'text/html');
   return doc.body.textContent || "";
}

function removeSpaces(str)
{
	//console.log("old version of the string: " + str);
	str = str.replace(/\s{2,}/g, ' ');
	str = str.replace(/\t/g, ' ');
	str = str.toString().trim().replace(/(\r\n|\n|\r)/g,"");
	//console.log("new version of the string: " + str);
	return str;
}

function addItem(){
	var addMe = true;
    var ul = document.getElementById("myList");
    var candidate = document.getElementById("candidate");
		
	var list = ul.childNodes;
	for(var i = 0; i < list.length; i++)
	{
		if(list[i].id == candidate.value)
		{
			addMe = false;
		}
	}	
		
	if(addMe)
	{
		//can only add a tag once
		var newValue = removeSpecial(candidate.value);
		var li = document.createElement("li");
		li.setAttribute('id',newValue);
		li.style.display = "inline";
		var b = document.createElement('button');
		
		b.setAttribute("value", newValue);
		b.setAttribute("id", newValue);
		b.setAttribute("style", "margin:5px;");
		
		console.log(newValue);
		
		b.setAttribute("onClick", "clickedBoy("+newValue+")");
		b.appendChild(document.createTextNode(candidate.value));
		
		li.appendChild(b);
		ul.appendChild(li);
		//availableTags.push(newValue);
		newTags.push(newValue);
	}
}

function clickedBoy(inputBoy)
{
	if(HTMLCollection.prototype.isPrototypeOf(inputBoy))
	{
		//console.log("we in chrome");
		//we need to get the li out of this HTMLCollection
		inputBoy = inputBoy.item(0);
		//console.log(inputBoy);
	}
	else
	{
		//console.log("we in firefox");
		//code stays the same
	}
	
	var ul = document.getElementById("myList");
	var list = ul.childNodes;
	for(var i = 0; i < list.length; i++)
	{
		//console.log(list[i]);
		if(list[i].id == inputBoy.id)
		{
			ul.removeChild(list[i]);
		}
	}	
}

 $(function() {
	 if($('body').is('.MemeUploadPage')){
		$( "#candidate" ).autocomplete({
	   appendTo: "#listContainer",
	   minLength: 0,
	   source: availableTags,
	   scroll: true,
	   focus: function( event, ui ) {
		  //$(this).autocomplete("search", "");
		  $( "#candidate" ).val( ui.item.label );
		 return false;
	   },
	   select: function( event, ui ) {
		  $( "#candidate" ).val( ui.item.label );
		  $( "#candidate-id" ).val( ui.item.value );
		  return false;
	   }
	})
		
	.data( "ui-autocomplete" )._renderItem = function( ul, item ) {
	   return $( "<li>" )
	   .data( "ui-autocomplete", item )
	   .append( "<a>" + item.label + "<br>" + "</a>" )
	   .appendTo( ul );
	};
	 }
 });
 
 // Home page setup
 function HomeOnLoad(){
	 showFeed();
	 MenuUpload();
 }
 
 
// function to show memes in order on the home page 
function showFeed(){
	var selectbox = document.getElementById("setting");
	var userSetting = selectbox.options[selectbox.selectedIndex].value;
	if(userSetting == "N2O"){
		var bigdiv = document.getElementById("feed");
		sendRequest("/getMemeNewestToOldest", null, (xhr)=>{
		bigdiv = document.getElementById("feed");
		bigdiv.innerHTML = xhr.responseText;
		breakUpMemesIntoButtons();
	});
	}
	if(userSetting =="O2N")
	{
		var bigdiv = document.getElementById("feed");
		sendRequest("/getMemeOldestToNewest", null, (xhr)=>{
		bigdiv = document.getElementById("feed");
		bigdiv.innerHTML = xhr.responseText;
		breakUpMemesIntoButtons();
	});
	}
}

function breakUpMemesIntoButtons()
{
	var feed = document.getElementById("feed");
	var elementals = feed.getElementsByTagName('img');

	for (var i = 0; i < elementals.length; i++)
	{
		var string = "image" + i;
		//console.log(string);
		elementals[i].setAttribute("class", "feedImage");
		elementals[i].setAttribute("id", string);
		elementals[i].setAttribute("visibility", "visible");
		elementals[i].setAttribute("onClick", "getCommentSection("+string+")");		
	}
}

function onLoadCommentSection()
{
	console.log("hello there kenobi");
}

function getCommentSection(id)
{
	if(HTMLCollection.prototype.isPrototypeOf(id))
	{
		//console.log("we in chrome");
		//we need to get the li out of this HTMLCollection
		id = id.item(0);
	}
	else
	{
		//console.log("we in firefox");
		//code stays the same
		id = id.getAttribute("id");
	}
	console.log(id);
	var imageToQuestion = document.getElementById(id)
	var postID = imageToQuestion.getAttribute("tag")
}

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

function generatedMemeSaveCheck() 
{
	document.getElementById("uploadImage").required = false;
	console.log(document.getElementById("uploadImage").type);
	if (localStorage.getItem("transfer_img") == null)
		console.log("No pre-Generated image saved");
	else
	{
		//var oFReader = new FileWriter();
		//oFReader.readAsDataURL(document.getElementById("uploadImage").files[0]);
	   
		//oFReader.onload = function (oFREvent) {
		//	document.getElementById("uploadPreview").src = oFREvent.target.result;
		//	document.getElementById("fullImage").src = oFREvent.target.result;
			
		var dataImage = localStorage.getItem("transfer_img");
		//var meme_img = new Image();
		//meme_img.src = dataImage;
		console.log("Pre-generated image found : " + dataImage);
		document.getElementById("uploadPreview").src = dataImage;
		document.getElementById("fullImage").src = dataImage;
		//document.getElementById("uploadImage").files[0] = toDataURL(dataImage);
		//localStorage.setItem("transfer_img", null);
	}
}

function uploadSetup()
{
	setUp();
	generatedMemeSaveCheck();
	console.log("exited uploadSetup");
}