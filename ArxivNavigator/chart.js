// Copyright 2021 Observable, Inc.
// Released under the ISC license.
// https://observablehq.com/@d3/force-directed-graph
function ForceGraph(
    {
        nodes, // an iterable of node objects (typically [{id}, …])
        links, // an iterable of link objects (typically [{source, target}, …])
    },
    {
        nodeId = (d) => d.id, // given d in nodes, returns a unique identifier (string)
        nodeGroup, // given d in nodes, returns an (ordinal) value for color
        nodeGroups, // an array of ordinal values representing the node groups
        nodeTitle, // given d in nodes, a title string
        nodeFill = "currentColor", // node stroke fill (if not using a group color encoding)
        nodeStroke = "#fff", // node stroke color
        nodeStrokeWidth = 1.5, // node stroke width, in pixels
        nodeStrokeOpacity = 1, // node stroke opacity
        nodeRadius = 5, // node radius, in pixels
        nodeStrength,
        linkSource = ({ source }) => source, // given d in links, returns a node identifier string
        linkTarget = ({ target }) => target, // given d in links, returns a node identifier string
        linkStroke = "#999", // link stroke color
        linkStrokeOpacity = 0.6, // link stroke opacity
        linkStrokeWidth = 1.5, // given d in links, returns a stroke width in pixels
        linkStrokeLinecap = "round", // link stroke linecap
        linkStrength,
        colors = d3.schemeTableau10, // an array of color strings, for the node groups
        width = 640, // outer width, in pixels
        height = 400, // outer height, in pixels
        invalidation, // when this promise resolves, stop the simulation
    } = {}
) {
    // Compute values.
    const N = d3.map(nodes, nodeId).map(intern);
    const LS = d3.map(links, linkSource).map(intern);
    const LT = d3.map(links, linkTarget).map(intern);
    if (nodeTitle === undefined) nodeTitle = (_, i) => N[i];
    const T = nodeTitle == null ? null : d3.map(nodes, nodeTitle);
    const G = nodeGroup == null ? null : d3.map(nodes, nodeGroup).map(intern);
    const W = typeof linkStrokeWidth !== "function" ? null : d3.map(links, linkStrokeWidth);
    const L = typeof linkStroke !== "function" ? null : d3.map(links, linkStroke);
    const R = typeof nodeRadius !== "function" ? null : d3.map(nodes, nodeRadius); //  Function takes appropriate
    //  node attribute and converts
    //  it to number.

    // Replace the input nodes and links with mutable objects for the simulation.
    nodes = d3.map(nodes, (_, i) => ({ id: N[i] }));
    links = d3.map(links, (_, i) => ({ source: LS[i], target: LT[i] }));

    // Compute default domains.
    if (G && nodeGroups === undefined) nodeGroups = d3.sort(G);

    // Construct the scales.
    const color = nodeGroup == null ? null : d3.scaleOrdinal(nodeGroups, colors);

    // Construct the forces.
    const forceNode = d3.forceManyBody();
    const forceLink = d3.forceLink(links).id(({ index: i }) => N[i]);

    if (nodeStrength !== undefined) forceNode.strength(nodeStrength);
    else {
        if (typeof nodeRadius !== "function") {
            if (nodeRadius !== undefined) forceNode.strength(nodeRadius);
        } else {
            console.assert(R !== undefined);
            //  Repulsion is proportional to the number of articles ("mass" of node).
            forceNode.strength(({ index: i }) => {
                /*console.log(R[i]);*/ return -1 * Math.pow(R[i], 2);
            });
        }
    }

    if (linkStrength !== undefined) forceLink.strength(linkStrength);

    const simulation = d3
        .forceSimulation(nodes)
        .force("link", forceLink)
        .force("charge", forceNode)
        .force("center", d3.forceCenter())
        .force(
            "collision",
            d3.forceCollide().radius(({ index: i }) => R[i])
        ) //  Circles don't overlap.
        .on("tick", ticked);

    const viewBoxX = -width / 2;
    const viewBoxY = -height / 2;

    const svg = d3.create("svg").attr("width", width).attr("height", height).attr("viewBox", [viewBoxX, viewBoxY, width, height]).attr("style", "max-width: 100%; height: auto; height: intrinsic;");

    const link = svg
        .append("g")
        .attr("stroke", typeof linkStroke !== "function" ? linkStroke : null)
        .attr("stroke-opacity", linkStrokeOpacity)
        .attr("stroke-width", typeof linkStrokeWidth !== "function" ? linkStrokeWidth : null)
        .attr("stroke-linecap", linkStrokeLinecap)
        .selectAll("line")
        .data(links)
        .join("line");

    const node = svg
        .append("g")
        .attr("fill", nodeFill)
        .attr("stroke", nodeStroke)
        .attr("stroke-opacity", nodeStrokeOpacity)
        .attr("stroke-width", nodeStrokeWidth)
        .selectAll("circle")
        .data(nodes)
        .join("circle")
        .attr("r", typeof nodeRadius !== "function" ? nodeRadius : 5)
        .call(drag(simulation));

    if (W) link.attr("stroke-width", ({ index: i }) => W[i]);
    if (L) link.attr("stroke", ({ index: i }) => L[i]);
    if (G) node.attr("fill", ({ index: i }) => color(G[i]));
    if (R) node.attr("r", ({ index: i }) => R[i]);
    if (T) node.append("title").text(({ index: i }) => T[i]);
    if (invalidation != null) invalidation.then(() => simulation.stop());

    function intern(value) {
        return value !== null && typeof value === "object" ? value.valueOf() : value;
    }

    function ticked() {
        link.attr("x1", (d) => d.source.x)
            .attr("y1", (d) => d.source.y)
            .attr("x2", (d) => d.target.x)
            .attr("y2", (d) => d.target.y);

        node.attr("cx", (d) => d.x).attr("cy", (d) => d.y);
    }

    function drag(simulation) {
        function dragstarted(event) {
            if (!event.active) simulation.alphaTarget(0.3).restart();
            event.subject.fx = event.subject.x;
            event.subject.fy = event.subject.y;
        }

        function dragged(event) {
            event.subject.fx = event.x;
            event.subject.fy = event.y;
        }

        function dragended(event) {
            if (!event.active) simulation.alphaTarget(0);
            event.subject.fx = null;
            event.subject.fy = null;
        }

        return d3.drag().on("start", dragstarted).on("drag", dragged).on("end", dragended);
    }

    const fontSize = $(window).innerWidth() / 40;

    const graphTitle = svg
        .append("text")
        .attr("x", viewBoxX + width / 2)
        .attr("y", viewBoxY + fontSize * 2)
        .attr("text-anchor", "middle")
        .style("font-size", fontSize.toString() + "px")
        //.style("text-decoration", "underline")
        .text("Arxiv.org topics by number of articles");

    return Object.assign(svg.node(), { scales: { color } });
}
