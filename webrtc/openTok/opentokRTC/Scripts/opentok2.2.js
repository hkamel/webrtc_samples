
var apiKey = "";  
var sessionId = "";  
var token = "" ;   
		
var session;
var publisher;
		
var VIDEO_WIDTH = 361;
var VIDEO_HEIGHT = 347;

		 
var _selfstream;
var  subscribers = new Array();
var _streams = new Array();
var _connections = new Array();

var Opentok = {
    connect: function (openTok) {


        OT.on("exception", exceptionHandler);

       
        OT.setLogLevel(OT.DEBUG);

        if (!(OT.checkSystemRequirements())) {
            alert("You don't have the minimum requirements to run this application.");
        } else {

            sessionId = openTok.SessionId;
            apiKey = openTok.ApiKey;
            token = openTok.Token;

            session = OT.initSession(sessionId);	// Initialize session
            session.connect(apiKey, token);
            // Add event listeners to the session

            session.on('sessionConnected', sessionConnectedHandler);
            session.on('sessionDisconnected', sessionDisconnectedHandler);
            session.on('connectionCreated', connectionCreatedHandler);
            session.on('connectionDestroyed', connectionDestroyedHandler);
            session.on('streamCreated', streamCreatedHandler);
            session.on('streamDestroyed', streamDestroyedHandler);
           // session.on("signal", signalEventHandler);


        }
    }
    ,
disconnect: function () {
    stopPublishing();
    session.disconnect() ;
    session.off('sessionConnected', sessionConnectedHandler);
    session.off('streamCreated', streamCreatedHandler);
    session.off('streamDestroyed', streamDestroyedHandler);
    session.off('connectionCreated', connectionCreatedHandler);
    //session.off("signal", signalEventHandler);
    OT.off("exception", exceptionHandler);
    session.off('sessionDisconnected', sessionDisconnectedHandler);
    publisher = null;

   
			 
}
}




// Called when user wants to start publishing to the session
function startPublishing() {
    if (!publisher) {
        var name= document.getElementById("txtName").value;
        var parentDiv = document.getElementById("myCamera");
        var publisherDiv = document.createElement('div'); // Create a div for the publisher to replace
        publisherDiv.setAttribute('id', 'opentok_publisher');
        parentDiv.appendChild(publisherDiv);
        var publisherProps = {width: VIDEO_WIDTH, height: VIDEO_HEIGHT, name: name};
        publisher = OT.initPublisher(apiKey, publisherDiv.id, publisherProps);  // Pass the replacement div id and properties
        session.publish(publisher);
        publisher.on("streamCreated", function (event) {  //access the self video 
            _selfstream = event.stream;
        });
				 
    }
}

function stopPublishing() {
    if (publisher) {
        session.unpublish(publisher);
    }
    publisher = null;

			 
}

		
function sessionConnectedHandler(event) {
			 
    startPublishing();
   
}

function streamCreatedHandler(event) {
			 
    addStream(event.stream);
                          
			 
}

var OnlineUsers = {
    addButton: function (user) {


        if (!document.getElementById("btn_" + user.ConnectionId)) {
            var button = document.createElement("input");
            var buttonContainer = document.getElementById("onlineusers");

            button.setAttribute("id", "btn_" + user.ConnectionId);
            button.setAttribute("type", "button");
            button.setAttribute("value", "Call " + user.Name);
            button.setAttribute("onclick", "begincallsignal(this)");
            buttonContainer.appendChild(button);
            buttonContainer.appendChild(document.createElement("br"));
             
        }
    }
,
    removeButton: function (user) {

        var btn = document.getElementById("btn_" + user.ConnectionId)
        var buttonContainer = document.getElementById("onlineusers");
      
        if (btn) {
            buttonContainer.removeChild(btn);

        }
    }
,
    removeAllButtons: function () {
        var buttonContainer = document.getElementById("onlineusers");
        buttonContainer.innerHTML = '';
    }
}



function endCall(obj, label) {

    
    console.log(obj.value);
    obj.value = label;
    obj.setAttribute("onclick", "begincallsignal(this)");
   
}

function endcallsignal(obj, label)
{
    var rtc = $.connection.rTCHub;
    endCall(obj, label);
    var connectionid = obj.id.replace("btn_", "")
    rtc.server.endCall(connectionid);
}

function begincallsignal(obj)
{
    var rtc = $.connection.rTCHub;
    beginCall(obj);
    var connectionid= obj.id.replace("btn_", "")
    rtc.server.beginCall(connectionid);

}


function beginCall(obj) {

   
     

    obj.setAttribute("onclick", "endcallsignal(this,'" + obj.value + "')");
    obj.value = 'End Call';
 
}


 
function streamDestroyedHandler(event) {
    // This signals that a stream was destroyed. Any Subscribers will automatically be removed.
    // This default behaviour can be prevented using event.preventDefault()
    removeStream(event.stream);
}

function sessionDisconnectedHandler(event) {
    // This signals that the user was disconnected from the Session. Any subscribers and publishers
    // will automatically be removed. This default behaviour can be prevented using event.preventDefault()

   
   
			 
}



function connectionDestroyedHandler(event) {
    // This signals that connections were destroyed
}

function connectionCreatedHandler(event) {
    // This signals new connections have been created.
}

/*
If you un-comment the call to OT.setLogLevel(), above, OpenTok automatically displays exception event messages.
*/
function exceptionHandler(event) {
    alert("Exception: " + event.code + "::" + event.message);
}

//--------------------------------------
//  HELPER METHODS
//--------------------------------------

function addStream(stream) {
    // Check if this is the stream that I am publishing, and if so do not publish.
    if (stream.connection.connectionId == session.connection.connectionId) {
        return;
    }
    var subscriberDiv = document.createElement('div'); // Create a div for the subscriber to replace
    subscriberDiv.setAttribute('id', stream.streamId); // Give the replacement div the id of the stream as its id.
    document.getElementById("subscribers").appendChild(subscriberDiv);
    var subscriberProps = {width: VIDEO_WIDTH, height: VIDEO_HEIGHT};
    subscribers[stream.streamId] = session.subscribe(stream, subscriberDiv.id, subscriberProps);
}


function removeStream(stream)
{
    session.unsubscribe(subscribers[stream.streamId]);
}
     
var acceptCallBox =
    {
        show: function () { $('#acceptCallBox').show(); },
        hide: function () { $('#acceptCallBox').hide(); this.message(''); },
        IsVisible: function () { if ($('#acceptCallBox').is(':visible')) { return true; } else { return false; } },
        message: function (strmessage)
        { $('#acceptCallLabel').html(strmessage); },
         

    }
var ringing = {

    play: function () {
        var audio = document.getElementById("rigning");
        audio.muted = false;
        audio.play();
    },
    mute: function () { document.getElementById("rigning").muted = true; }

}