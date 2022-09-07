# ArxivNavigator

Interactive visualizations of Arxiv metadata. 

<script>
    function resizeIframe(data) {
        document.getElementById('topicsFrame').style.height = data + 'px';
    };
 
    function sendLocation(){
        //  Notifies child window about parent name.
        var win = window.frames.topicsFrame; 
        win.postMessage(window.location.origin, 'https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html');
    };
 
    var messageEventHandler = function(event){
        //  Updates frame size according to child window size.
        if(event.origin === 'https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html'){
            resizeIframe(event.data);
        }
    }; 
 
    window.addEventListener('message', messageEventHandler, false);
</script>

 <iframe src="https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html" 
     title="Arxiv topics by the number of written articles." 
     id="topicsFrame" scrolling="no" style="width:100%; border:none;" 
     name="topicsFrame" onload="sendLocation();>
 </iframe> 