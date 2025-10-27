var parentDestination = null;
var inFrame = false;

function isDefined(obj) {
    if (obj !== undefined && obj !== null) return true;
    else return false;
}

function updateParentSize() {
    //  Send message with size to parent window.
    if (isDefined(parentDestination)) {
        window.parent.postMessage({ height: document.body.scrollHeight, width: document.body.scrollWidth },
            parentDestination);
    }
}

(function () {
    var parentEventHandler = function (event) {
        //  Handles messages from parent window.
        //	Destination should not be empty string.
        if (isDefined(event.data) &&
           (event.data.search("http://") != -1 || event.data.search("https://") != -1)) {
            parentDestination = event.data;

            inFrame = true;
            //  When page is opened in frame graph size is decreased.
            redrawTopicGraph();
            updateParentSize();
        }
        else
            parentDestination = null;
    };
    window.addEventListener('message', parentEventHandler, false);
})();
