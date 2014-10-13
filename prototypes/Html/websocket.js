
var wsUri = "ws://192.168.0.25:8081"; 
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
	
}



function onOpen(evt) {
	$("#divData").html("Connected");	
}

function onClose(evt) {
	$("#divData").html("Disconnected");

}

//Send text to Server
function sendTextToWebSocket(msg){
    websocket.send(msg);
}


                
function onMessage(evt) {
 	$("#divData").html(evt.data);
}
















