"use strict";
		
function setUp()
{
	console.log("setup");
	var ul = document.getElementById("myList");
	
	//console.log(ul);
	
	var li = document.createElement("li");
	li.innerHTML = "Tag: ";
	li.style.display = "inline";
	
	//console.log(li);
	
	ul.appendChild(li);
	
	//console.log(ul);
}

function encodeHTML(s) {
    return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/"/g, '&quot;').replace(/>/g, '&gt;');
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
		var li = document.createElement("li");
		li.setAttribute('id',candidate.value);
		li.style.display = "inline";
		var b = document.createElement('button');
		
		var newValue = encodeHTML(candidate.value);
				
		b.setAttribute("value", newValue);
		b.setAttribute("id", newValue);
		b.setAttribute("onClick", "clickedBoy("+newValue+")");
		b.appendChild(document.createTextNode(newValue));
		
		//console.log(b);
		
		li.appendChild(b);
		//console.log(li);
		
		ul.appendChild(li);
		
		availableTags.push(newValue);
	}
}

function clickedBoy(inputBoy)
{
	console.log(inputBoy);
	
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