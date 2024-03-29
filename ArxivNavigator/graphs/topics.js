////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// File "topics.js"
// Copyright © Dmitry Morozov 2022
// This script displays topic graph.
// If you want to use this file please contact me by dvmorozov@hotmail.com.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getTopicByid(topicId) {
    for (let i = 0; i < topics.nodes.length; i++) {
        if (topics.nodes[i].id === topicId) {
            return topics.nodes[i];
        }
    }
    return null;
}

function showTopicPopup(topicId) {
    //  Closes dialog if it was open before,
    //  otherwise content is not displayed.
    if ($("#popup").hasClass("ui-dialog-content") &&
        $("#popup").dialog("isOpen"))
        $("#popup").dialog("close");

    var dialog = $("#popup").dialog({
        title: topicId,
        resizable: false,
        width: 1000,
        height: 618,
        open: function(event, ui) {
            //  Shows tabs.
            $("#tabs").tabs({
                heightStyle:"fill",
                collapsible:true,
                activate: function( event, ui ) {
                    if (event.type == 'tabsactivate') {
                        const topic = getTopicByid(topicId);

                        if ($(event.currentTarget).attr("href") == '#tabs-1') {
                            redrawArticleList(topic);
                        }
                        else if ($(event.currentTarget).attr("href") == '#tabs-2') {
                            redrawPublishingRateChart(topic);
                        }
                    }
                }
            });

            const topic = getTopicByid(topicId);

            redrawArticleList(topic);
            redrawPublishingRateChart(topic);
        }
    });

    //  Position must be set after opening the dialog.
    dialog.dialog('option', 'position', { my: "center", at: "center", of: window });
}

function getArticleList(topic) {
    var articleList = [];

    console.assert(topic !== null);
    //  Fills list of last articles.
    //  Converts data to the form appropriate for DataTables.
    for (let j = 0; j < topic.last_articles.length; j++) {
        const article = topic.last_articles[j];
        const articleLink = '<a href="http://arxiv.org/abs/' + article.id +
                            '" target="_blank">' + article.title + '</a>';
        var articleData = [];

        articleData.push(article.last_version_date);
        articleData.push(articleLink);
        //articleData.push(article.id);

        articleList.push(articleData);
    }

    return articleList;
}

function redrawArticleList(topic) {
    const articleList = getArticleList(topic);

    //  Removes all content and creates new table. Otherwise, data is not cleared.
    $('#content').children().remove();
    //  Creates placeholder for table.
    $('#content').append('<table>');

    //  Data table should be created from the event handler,
    //  otherwise column width aren't set up properly.
    $('#content > table').DataTable({
        data: articleList,
        columns: [
            { title: 'Published', "width": "20%" },
            { title: 'Title' },
            //{ title: 'Id' },
        ],
        info: false,
        ordering: true,
        paging: false,
        searching: false,
        scrollX: true,
        scrollY: true,
        autoWidth: false,
    });
}

function redrawTopicGraph(inFrame) {
    //  Removes all content from graph element.
    $("#force_graph").children().remove();

    graphWidth = $('#force_graph').width();
    graphHeight = $('#force_graph').height();

    graph = ForceGraph(topics, {
        nodeGroup: d => d.group,
        nodeTitle: d => {
            const last_article = d.last_articles.length > 0 ? d.last_articles[0].title : '';
            const publishing_date = d.last_articles.length > 0 ? d.last_articles[0].last_version_date : '';

            return `Topic: \"${d.id}\"\nNumber of articles: ${d.value}\n` +
                   `Last article: \"${last_article}\"\nPublished: ${publishing_date}\n` +
                   `Click the node to view list of articles.`
        },
        width: graphWidth,
        height: graphHeight,
        invalidation: null,                         //  a promise to stop the simulation when the cell is re-run
        graphTitle: "Number of articles by topic\n" +
            "Totally " + topics.article_count + " articles\n" +
            "Last updated " + topics.updated,
        inFrame: inFrame,                           //  Controls text drawing.
        showPopup: showTopicPopup
    });

    document.getElementById('force_graph').appendChild(graph);
}

function redrawPublishingRateChart(topic) {
    //  Removes all content from bar chart tab.
    $("#bar_chart").children().remove();

    chartWidth = $('#tabs-2').width();
    chartHeight = $('#tabs-2').height();

    $("#bar_chart").width(chartWidth);
    $("#bar_chart").height(chartHeight);

    chart = BarChart(topic.articles_by_year, {
        x: (d) => d.year,
        y: (d) => d.articles_count,
        xDomain: d3.groupSort(
            topic.articles_by_year,
            ([d]) => d.year,
            (d) => d.year
        ),
        yDomain: d3.extent(
            topic.articles_by_year,
            (d) => d.articles_count
        ),
        yFormat: d3.format(".0f"),
        yLabel: "↑ Number of published articles",
        width : chartWidth,
        height: chartHeight,
        color: "steelblue",
    });

    document.getElementById('bar_chart').appendChild(chart);
}
