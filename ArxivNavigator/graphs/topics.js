////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// File "topics.js"
// Copyright © Dmitry Morozov 2022
// This script displays topic graph.
// If you want to use this file please contact me by dvmorozov@hotmail.com.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showTopicPopup(topicId) {
    var articleList = [];
    //  Fills list of last articles.
    for (let i = 0; i < topics.nodes.length; i++) {
        if (topics.nodes[i].id === topicId) {
            //  Converts data to the form appropriate for DataTables.
            for (let j = 0; j < topics.nodes[i].last_articles.length; j++) {
                const article = topics.nodes[i].last_articles[j];
                const articleLink = '<a href="http://arxiv.org/abs/' + article.id +
                                    '" target="_blank">' + article.title + '</a>';
                var articleData = [];

                articleData.push(article.last_version_date);
                articleData.push(articleLink);
                //articleData.push(article.id);

                articleList.push(articleData);
            }

            //  Removes all content and creates new table. Otherwise, data is not cleared.
            $('#popup > #content').children().remove();
            //  Creates placeholder for table.
            $('#popup > #content').append('<table>');
        }
    }

    //  Closes dialog if it was open before,
    //  otherwise content is not displayed.
    if ($("#popup").hasClass("ui-dialog-content") &&
        $("#popup").dialog("isOpen"))
        $("#popup").dialog("close");

    var dialog = $("#popup").dialog({
        title: topicId,
        resizable: false,
        //  Height is set up by content.
        width: 1000,
        open: function(event, ui) {
            //  Data table should be created from the event handler,
            //  otherwise column width aren't set up properly.
            var table = $('#popup > #content > table').DataTable({
                data: articleList,
                columns: [
                    { title: 'Published', "width": "20%" },
                    { title: 'Title' },
                    //{ title: 'Id' },
                ],
                info: false,
                ordering: false,
                paging: false,
                searching: false,
                scrollX: false,
                scrollY: false,
                autoWidth: false,
            });
        }
    });

    //  Position must be set after opening the dialog.
    dialog.dialog('option', 'position', { my: "center", at: "center", of: window });
}

function redrawTopicGraph(inFrame) {
    var graph = document.getElementById('visualisation');
    //  Removes graph.
    while (graph.hasChildNodes()) {
        graph.removeChild(graph.firstChild);
    }

    graphWidth = $('svg#visualisation').width();
    graphHeight = $('svg#visualisation').height();

    chart = ForceGraph(topics, {
        nodeGroup: d => d.group,
        //nodeStrength: -200,                       //  This could be a number.
                                                    //  Negative value means repulsion, positive - attraction.
        nodeTitle: d => {
            const last_article = d.last_articles.length > 0 ? d.last_articles[0].title : '';
            const publishing_date = d.last_articles.length > 0 ? d.last_articles[0].last_version_date : '';

            return `Topic: \"${d.id}\"\nNumber of articles: ${d.value}\n` +
                   `Last article: \"${last_article}\"\nPublished: ${publishing_date}\n` +
                   `Click the node to view list of articles.`
        },
        //linkStroke,                               //  Function or value providing stroke value.
        //linkStrokeWidth: 2,
        //nodeRadius: 5,
        width: graphWidth,
        height: graphHeight,
        invalidation: null,                         //  a promise to stop the simulation when the cell is re-run
        graphTitle: "Number of articles by topic (" + topics.article_count + " articles processed)",
        inFrame: inFrame,                           //  Controls text drawing.
        showPopup: showTopicPopup
    });

    document.getElementById('visualisation').appendChild(chart);
}
