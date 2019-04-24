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
	var ourMeme = document.getElementById("fullImage");
	var fd = new FormData();
    var fileToUpload = document.getElementById("uploadImage").files[0];
    if( fileToUpload ){
        fd.append( "meme", fileToUpload );
		fd.append( "tags", tags);
		sendRequest( "uploadMeme", fd, () => {
		window.location.replace("index.html"); //meme spread page will go here
    });
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

function addComment(){
	var fd = new FormData();
    var ul = document.getElementById("commentList");
    var candidate = document.getElementById("myComment");
	
	var newValue = removeSpecial(candidate.value);
	newValue = removeSpaces(newValue);
	
	fd.append("comment", newValue);
	
	var li = document.createElement("li");
	li.setAttribute('id',newValue);
	var b = document.createElement('button');
	
	b.setAttribute("value", newValue);
	b.setAttribute("id", newValue);
	b.setAttribute("style", "margin:5px;");
	b.setAttribute("onClick", "clickedComment("+newValue+")");
	b.appendChild(document.createTextNode(candidate.value));
	
	li.appendChild(b);
	ul.appendChild(li);	
	
	sendRequest("addComment", fd, )
	
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
}

function clickedComment(inputBoy)
{
	//firefox inputBoy is li, chrome inputBoy is htmlcollection envolping a li
	
	if(HTMLCollection.prototype.isPrototypeOf(inputBoy))
	{
		//console.log("we in chrome");
		//we need to get the li out of this HTMLCollection
		inputBoy = inputBoy.item(0);
	}
	else
	{
		//console.log("we in firefox");
		//code stays the same
	}
		
	var ul = document.getElementById("commentList");
	var list = ul.childNodes;
	for(var i = 0; i < list.length; i++)
	{
		var temp = list[i].id;
		if(temp != undefined)
			temp = removeSpaces(temp);
		//console.log("checking vs: " + temp);
		if(temp == inputBoy.id)
		{
			ul.removeChild(list[i]);
		}
	}	
}
function doRating(val)
{
	var inp = document.getElementById("Rating_"+val);
	var fd = new FormData();
	fd.append( "Post_Id", inp.name );
	fd.append( "Rating", val );
	sendRequest("doRating", fd, undefined, "POST");
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
	var bigdiv = document.getElementById("feed");
	sendRequest("/topTen", null, (xhr)=>{
		bigdiv = document.getElementById("feed");
		bigdiv.innerHTML = xhr.responseText;
	});
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
	if (localStorage.getItem("transfer_img") == null)
		console.log("No pre-Generated image saved");
	else
	{
		var dataImage = localStorage.getItem("transfer_img");
		var meme_img = new Image();
		meme_img.src = dataImage;
		console.log("Pre-generated image found : " + dataImage);
		document.getElementById("uploadPreview").src = dataImage;
		document.getElementById("fullImage").src = dataImage;
		document.getElementById("uploadImage") = "Hoe";
		localStorage.setItem("transfer_img", null);
	}
}

function uploadSetup()
{
	setUp();
	generatedMemeSaveCheck();
	console.log("exited uploadSetup");
}