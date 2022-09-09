# ArxivNavigator

Look at [arxiv client for Android and iOS](https://dvmorozov.github.io/arxiv/).

<link rel="shortcut icon" href="https://dvmorozov.github.io/arxiv/ArxivNavigator/favicon.ico">

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
<iframe src="https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html"
     title="Arxiv topics by the number of written articles."
     id="topicsFrame" scrolling="no" style="width:100%; border:none;"
     name="topicsFrame" onload="sendLocation();">
</iframe>
</div>

The visualization uses modified [force-directed graph](https://observablehq.com/@d3/force-directed-graph) powered by [d3.js](https://d3js.org/).

Data has been collected by [modified fork](https://github.com/dvmorozov/arxiv-public-datasets) of [arxiv-public-datasets metadata collector](https://github.com/mattbierbaum/arxiv-public-datasets).
 
JSON parsing has been implemented with [ijson](https://pypi.org/project/ijson/).
