function resizeIframe(data) {
    document.getElementById('topicsFrame').style.height = data.height + 'px';
    document.getElementById('topicsFrame').style.width = data.width + 'px';
};

function sendLocation(){
    //  Notifies child window about parent location.
    var win = window.frames.topicsFrame;
    win.postMessage(window.location.origin, 'https://dvmorozov.github.io');
};

var messageEventHandler = function(event){
    //  Updates frame size according to child window size.
    if(event.origin === 'https://dvmorozov.github.io'){
        resizeIframe(event.data);
    }
};

window.addEventListener('message', messageEventHandler, false);
