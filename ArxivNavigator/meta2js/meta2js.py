########################################################################################################################
# File "meta2js.py"
# Copyright © Dmitry Morozov 2022
# This script converts downloaded arxiv metadata into graph data suitable for 3D.js.
# Script extracts topic data (article categories) for representation as force graph.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import gzip
import ijson
from topic_link import *
from datetime import datetime

processed_article_count = 0
updated = ""


def finish_parsing(write_to_file):
    global updated
    links_json = generate_links_json()
    nodes_json = generate_topics_json()

    print('link count', '=>', str(get_link_count()))
    print('topic count', '=>', str(get_topic_count()))
    print('articles', '=>', str(processed_article_count))
    print('updated', '=>', str(updated))

    topics = 'var topics = {' + nodes_json + ', ' + links_json + ', ' \
        'link_count: "' + str(get_link_count()) + '", ' \
        'topic_count: "' + str(get_topic_count()) + '", ' \
        'article_count: "' + str(processed_article_count) + '", ' \
        'updated: "' + str(updated) + '"};'

    # File is opened in text mode.
    with gzip.open(write_to_file, 'wt', encoding='utf-8') as gzip_file:
        gzip_file.write(topics)
        gzip_file.close()


def extract_topics_data():
    global processed_article_count, updated

    processed_article_count = 0
    #   File has JSON-format. Original name is saved for convenience.
    metadata = ijson.parse(open('../data/arxiv-public-datasets', 'r', encoding='utf8'))
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

        last_version_date = max(version_dates)

        for source_id in categories[0].split():
            topic = add_unique_topic(source_id)
            topic.add_article(Article(article["id"], article["title"], last_version_date))

            for target_id in categories[0].split():
                if source_id != target_id:
                    add_unique_link(source_id, target_id)

        processed_article_count += 1
        if processed_article_count % 10000 == 0:
            print('Processed: ', str(processed_article_count))

    # Reads the time when data has been collected.
    # New parser should be created, otherwise another type of objects is not returned.
    metadata = ijson.parse(open('../data/arxiv-public-datasets', 'r', encoding='utf8'))
    updated_items = ijson.items(metadata, 'updated')
    for updated_item in updated_items:
        updated = updated_item

    finish_parsing('../data/topics.js.gz')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_topics_data()
