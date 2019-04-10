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

function updateMyPic(){
    document.getElementById("pic").src="getMyProfilePic?junk="+Date.now();
}


function login(){
    sendRequest("doLogin",undefined, () => {
        updateMyPic();
    });
}

function logout(){
    sendRequest("doLogout", undefined, () => {
        updateMyPic();
    });
}

function changePic(){
    var fd = new FormData();
    var fileToUpload = document.getElementById("blah").files[0];
    if( fileToUpload ){
        fd.append( "pic", fileToUpload );
    }
    sendRequest( "setMyProfilePic", fd, () => {
        updateMyPic();
    });
}
