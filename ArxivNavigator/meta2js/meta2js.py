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
from month import *


processed_article_count = 0
updated = ""


def write_topics_to_js(file_path):
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
    with gzip.open(file_path, 'wt', encoding='utf-8') as gzip_file:
        gzip_file.write(topics)
        gzip_file.close()


def write_months_to_js(file_path):
    months_js = 'var months = ['
    first_month = True

    for month_name in months.keys():
        month = months[month_name]

        if not first_month:
            months_js += ', '

        month_js = '{"year": "' + str(month.get_year()) + '", "month": "' + month.get_month() + '", "article_ids": ['

        article_ids = month.get_article_ids()
        first_article_id = True

        for article_id in article_ids:
            if not first_article_id:
                month_js += ', '
            month_js += '"' + article_id + '"'
            first_article_id = False

        month_js += ']}'

        months_js += month_js
        first_month = False

    months_js += '];'

    # File is opened in text mode.
    with gzip.open(file_path, 'wt', encoding='utf-8') as gzip_file:
        gzip_file.write(months_js)
        gzip_file.close()


def extract_topics_data():
    global processed_article_count, updated

    # Creates months range to store
    create_months()

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
            date = datetime.datetime.strptime(version_date, '%a, %d %b %Y %H:%M:%S %Z')
            version_dates.append(date)

        last_version_date = max(version_dates)
        article_id = article["id"]
        # Add article to the set of month.
        month = get_month(last_version_date.year, last_version_date.month)
        month.add_article_id(article_id)

        for source_id in categories[0].split():
            topic = add_unique_topic(source_id)
            topic.add_article(Article(article_id, article["title"], last_version_date))

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

    write_topics_to_js('../data/topics.js.gz')
    write_months_to_js('../data/months.js.gz')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_topics_data()
