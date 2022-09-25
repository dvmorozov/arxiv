########################################################################################################################
# File "meta2js.py"
# Copyright © Dmitry Morozov 2022
# This script converts downloaded arxiv metadata into graph data suitable for 3D.js.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import ijson
from article import *
from topic_link import *
from datetime import datetime

article_count = 0
updated = ''


def finish_parsing(write_to_file):
    links_json = generate_links_json()
    nodes_json = generate_topics_json()

    print('link count', '=>', str(get_link_count()))
    print('topic count', '=>', str(get_topic_count()))
    print('articles', '=>', str(article_count))

    topics = 'var topics = {' + nodes_json + ', ' + links_json + ', '   \
        'link_count: "' + str(get_link_count()) + '", '                 \
        'topic_count: "' + str(get_topic_count()) + '", '               \
        'article_count: "' + str(article_count) + '", '                 \
        'updated: "' + updated + '"};'

    textfile = open(write_to_file, "w")
    textfile.write(topics)
    textfile.close()


def extract_topics_data():
    global article_count

    metadata = ijson.parse(open('../data/arxiv-public-datasets.json', 'r'))
    #   Extracts categories
    articles = ijson.items(metadata, 'articles.item')

    for article in articles:
        #  All categories are represented by single string.
        categories = article["categories"]

        version_dates = []
        for version_date in article["versions_dates"]:
            #   Time format 'Mon, 2 Apr 2007 19:18:42 GMT'
            date = datetime.strptime(version_date, '%a, %d %b %Y %H:%M:%S %Z')
            version_dates.append(date)

        for source_id in categories[0].split():
            topic = add_unique_topic(source_id)
            topic.add_article(Article(article["id"], article["title"], max(version_dates)))

            for target_id in categories[0].split():
                if source_id != target_id:
                    add_unique_link(source_id, target_id)

        article_count += 1

    updated = ijson.items(metadata, 'updated.item')
    finish_parsing('../data/topics.js')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_topics_data()
