var WebSocketHandler = {
    OnStart: function(isTest){
            var message = "START;"+UTF8ToString(isTest);
            console.log("Send Message: "+message);
            window.crateSocket = new WebSocket('ws://localhost:8080/crate');
            window.crateSocket.onopen = function() {
                  window.crateSocket.send(message);
           };
    },
    OnFinish: function(filename,msg){
        var message = "FINISHED;"+UTF8ToString(filename)+";"+UTF8ToString(msg);
        console.log("Send Message: "+message);
        window.crateSocket.send(message);
        window.crateSocket.close();
        window.crateSocket = undefined;
    }
}
mergeInto(LibraryManager.library, WebSocketHandler);