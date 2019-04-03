"use strict";

var availableTags = [
  "ActionScript",
  "AppleScript",
  "Asp",
  "BASIC",
  "C",
  "C++",
  "Clojure",
  "COBOL",
  "ColdFusion",
  "Erlang",
  "Fortran",
  "Groovy",
  "Haskell",
  "Java",
  "JavaScript",
  "Lisp",
  "Perl",
  "PHP",
  "Python",
  "Ruby",
  "Scala",
  "Scheme"
];

function setUp()
{
	console.log("setup");
	var ul = document.getElementById("myList");
	
	//console.log(ul);
	
	var li = document.createElement("li");
	li.innerHTML = "Tags: ";
	li.style.margin = "margin:5px";
	li.style.display = "inline";
	//console.log(li);
	
	ul.appendChild(li);
	
	//console.log(ul);
}

function subMeme(){
	var ourMeme = document.getElementById("fullImage");
	
	var fd = new FormData();
	fd.append("image", ourMeme);
	
	var req = new XMLHttpRequest();
    req.addEventListener( "load", () => {
        if( req.readyState === 4 && req.status === 200 ){
            console.log("UploadMeme: "+req.responseText);
            if( callback != undefined )
                callback(req);
        }
    });
    req.open("POST", "UploadMeme" );
    req.send( fd );
	
	window.location.replace("Home.html"); //meme spread page will go here
}


function addComment(){
	var fd = new FormData();
    var ul = document.getElementById("commentList");
    var candidate = document.getElementById("myComment");
	
	var newValue = removeSpecial(candidate.value);
	newValue = removeSpaces(newValue);
	
	fd.append("comment", newValue);
	//fd.append("comment", newValue);
	
	var li = document.createElement("li");
	li.setAttribute('id',newValue);
	var b = document.createElement('button');
	
	b.setAttribute("value", newValue);
	b.setAttribute("id", newValue);
	b.setAttribute("style", "margin:5px;");
	b.setAttribute("onClick", "clickedComment("+newValue+")");
	b.appendChild(document.createTextNode(candidate.value));
	
	//console.log(b);
	
	li.appendChild(b);
	//console.log(li);
	
	ul.appendChild(li);	
	
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
	//console.log("onClick");
	//console.log(inputBoy.id);
	//console.log(inputBoy);
	//firefox inputBoy is li, chrome inputBoy is htmlcollection envolping a li
	
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
		
		//console.log(b);
		
		li.appendChild(b);
		//console.log(li);
		
		ul.appendChild(li);
		
		availableTags.push(newValue);
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
	   .append( "<a>" + item.label + "<br>" + "</a>" )
	   .appendTo( ul );
	};
 });