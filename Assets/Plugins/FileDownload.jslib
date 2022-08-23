var WebSocketHandler = {
    OnFinish: function(base64)
    {
        var socket = new WebSocket('ws://localhost:8080/crate');
        socket.send("FINISHED;"+base64);
        socket.close();
    }
};
mergeInto(LibraryManager.library, WebSocketHandler);