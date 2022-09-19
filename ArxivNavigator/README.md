# ArxivNavigator

Look at [arxiv client for Android and iOS](https://dvmorozov.github.io/arxiv/).

<script src="https://cdn.observableusercontent.com/npm/d3@7.6.1/dist/d3.min.js"></script>
<script src="https://cdn.observableusercontent.com/npm/marked@0.3.12/marked.min.js"></script>
<script src="https://cdn.observableusercontent.com/npm/htl@0.3.1/dist/htl.min.js"></script>
<script src="https://cdn.observableusercontent.com/npm/@observablehq/highlight.js@2.0.0/highlight.min.js"></script>

<script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
<script src="https://code.jquery.com/ui/1.13.1/jquery-ui.min.js" integrity="sha256-eTyxS0rkjpLEo16uXTS0uVCS4815lc40K2iVpWDvdSY=" crossorigin="anonymous"></script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/smoothness/jquery-ui.css">

<script src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>
<link rel="stylesheet" href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">

<script src="https://dvmorozov.github.io/arxiv/ArxivNavigator/data/topics.js" charset="utf-8"></script>

<script src="https://dvmorozov.github.io/arxiv/ArxivNavigator/graphs/bar-chart.js" charset="utf-8"></script>
<script src="https://dvmorozov.github.io/arxiv/ArxivNavigator/graphs/force-graph.js" charset="utf-8"></script>
<script src="https://dvmorozov.github.io/arxiv/ArxivNavigator/graphs/topics.js" charset="utf-8"></script>

<link rel="stylesheet" href="https://dvmorozov.github.io/arxiv/ArxivNavigator/graphs/main.css">
<link rel="shortcut icon" href="https://dvmorozov.github.io/arxiv/ArxivNavigator/graphs/favicon.ico">

<script>
    window.onload = function() {
        $("#force_graph").width(800);
        $("#force_graph").height(800);

        redrawTopicGraph(true);
    };

    $("body").css('overflow', 'scroll');
</script>

<div>
<svg id="force_graph"></svg>

<div id="popup" style="display: none;">
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Last Articles</a></li>
            <li><a href="#tabs-2">Publishing Rate</a></li>
        </ul>
        <div id="tabs-1">
            <p id="content">
            </p>
        </div>
        <div id="tabs-2">
            <svg id="bar_chart"></svg>
        </div>
    </div>
</div>
</div>

*Hold mouse on node to see topic identifier and related number of articles. 

Drag node to see relations. 

Click node to see the list of latest articles and the number of articles published per year.

Node opacity decreases exponentially since the time of last published article.*

<a href="https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html" target="_blank">Open graph in new window</a>

___

## References

The visualization uses modified [force-directed graph](https://observablehq.com/@d3/force-directed-graph) powered by [d3.js](https://d3js.org/).

Data has been collected by [modified fork](https://github.com/dvmorozov/arxiv-public-datasets) of [arxiv-public-datasets metadata collector](https://github.com/mattbierbaum/arxiv-public-datasets).
 
JSON parsing has been implemented with [ijson](https://pypi.org/project/ijson/).
