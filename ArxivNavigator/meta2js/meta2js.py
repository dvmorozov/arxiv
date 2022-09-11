########################################################################################################################
# File "topic.py"
# Copyright © Dmitry Morozov 2022
# This script converts downloaded arxiv metadata into graph data suitable for 3D.js.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import ijson
from topic_link import *

article_count = 0


def finish_parsing(write_to_file):
    links_json = generate_links_json()
    nodes_json = generate_topics_json()

    print('link count', '=>', str(get_link_count()))
    print('topic count', '=>', str(get_topic_count()))
    print('articles', '=>', str(article_count))

    topics = 'var topics = {' + nodes_json + ', ' + links_json + ', ' \
        'link_count: "' + str(get_link_count()) + '", ' \
        'topic_count: "' + str(get_topic_count()) + '", ' \
        'article_count: "' + str(article_count) + '"};'

    textfile = open(write_to_file, "w")
    textfile.write(topics)
    textfile.close()


def extract_topics_data():
    global article_count

    metadata = ijson.parse(open('../data/arxiv-public-datasets.json', 'r'))
    #   Extracts categories
    category_sets = ijson.items(metadata, 'articles.item.categories')

    for category_set in category_sets:
        #  All categories are represented by single string.
        for source in category_set[0].split():
            add_unique_topic(source)

            for target in category_set[0].split():
                if source != target:
                    add_unique_link(source, target)

        article_count += 1

    finish_parsing('../data/topics.js')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_topics_data()
