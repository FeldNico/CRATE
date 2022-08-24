var WebSocketHandler = {
    OnFinish: function(filename,msg){
        var message = "FINISHED;"+UTF8ToString(filename)+";"+UTF8ToString(msg);
        console.log(msg+" : "+message);
        var socket = new WebSocket('ws://localhost:8080/crate');
        socket.onopen = () => {
            socket.send(message)
            socket.close()
        }
    }
}
mergeInto(LibraryManager.library, WebSocketHandler);