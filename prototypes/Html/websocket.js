
// ws:// ipaddress and port /  server assigned endpoint / end point name ?id={projectID}
//var wsUri = "ws://192.168.2.100:5678/hmisocketserver_2/hmisocketserver?id=";
var wsUri = "ws://192.168.0.25:8081"; ///hmisocketserver_2/hmisocketserver?id=";
var websocket;

var isConnectionStateShowing = false;


function websocketConnect()
{
	websocket = new WebSocket(wsUri);
	websocket.onerror = function(evt) { onError(evt) };
	websocket.onopen = function(evt) { onOpen(evt) };
	websocket.onmessage = function(evt) { onMessage(evt) };
	websocket.onclose = function(evt) { onClose(evt) };
}

function onError(evt) {
	//alert(evt.data);
}



function onOpen(evt) {
	$("#divData").html("Connected");	
}

function onClose(evt) {
	$("#divData").html("Disconnected");

}

//Send text to Server
function sendTextToWebSocket(json){
    websocket.send(json);
}


                
function onMessage(evt) {
 	$("#divData").html(evt.data);
}
















