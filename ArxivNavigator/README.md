# ArxivNavigator

Look at [arxiv client for Android and iOS](https://dvmorozov.github.io/arxiv/).

<link rel="shortcut icon" href="https://dvmorozov.github.io/arxiv/ArxivNavigator/favicon.ico">

<div>
<svg id="visualisation"></svg>

<div id="popup">
    <h3 id="header"></h3>
    <p id="content">
    </p>
</div>
</div>

*Hold mouse on node to see topic identifier and related number of articles. Drag node to see relations.*

<a href="https://dvmorozov.github.io/arxiv/ArxivNavigator/topics.html" target="_blank">Open graph in new window</a>

___

## References

The visualization uses modified [force-directed graph](https://observablehq.com/@d3/force-directed-graph) powered by [d3.js](https://d3js.org/).

Data has been collected by [modified fork](https://github.com/dvmorozov/arxiv-public-datasets) of [arxiv-public-datasets metadata collector](https://github.com/mattbierbaum/arxiv-public-datasets).
 
JSON parsing has been implemented with [ijson](https://pypi.org/project/ijson/).
