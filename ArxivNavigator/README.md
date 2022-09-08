# ArxivNavigator

Interactive visualizations of Arxiv metadata. 

<link rel="shortcut icon" href="./graphs/favicon.ico">

<script language="JavaScript">
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
</script>

<div>
    <iframe src="./graphs/topics.html"
         title="Arxiv topics by the number of written articles."
         id="topicsFrame" scrolling="no" style="width:100%; border:none;"
         name="topicsFrame" onload="sendLocation();">
    </iframe>
</div>
 
