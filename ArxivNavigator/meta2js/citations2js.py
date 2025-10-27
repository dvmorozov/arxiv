########################################################################################################################
# File "citations2js.py"
# Copyright © Dmitry Morozov 2022
# This script extracts most influential articles from citation data.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import ijson
from citation import *
from anytree import Node, RenderTree


updated = ""
processed_article_count = 0
most_influential_articles = dict()


def get_max_citation_count():
    sorted_list = sorted(citations.items(), key=lambda x: x[1].get_citation_count(), reverse=True)
    max_citation_count = 0
    if len(sorted_list) > 0:
        key = sorted_list[0][0]
        max_citation_count = citations[key].get_citation_count()
    return max_citation_count


def print_summary_data():
    print('updated', '=>', str(updated))
    print('processed article count', '=>', str(processed_article_count))
    print('max article citation count', '=>', str(get_max_citation_count()))


def write_output(file_name):
    global updated
    topics = 'var most_influential_articles = {' + generate_citations_json() + ', ' \
        'processed_article_count: "' + str(processed_article_count) + '", ' \
        'updated: "' + str(updated) + '"};'

    text_file = open(file_name, "w", encoding='utf8')
    text_file.write(topics)
    text_file.close()


def save_only_most_influential_articles():
    global most_influential_articles
    #  Creates list sorted by cumulative number of citations in descending order.
    sorted_list = sorted(citations.items(), key=lambda x: x[1].get_citation_count(), reverse=True)
    max_citation_count = get_max_citation_count()
    most_influential_articles.clear()

    for item in list(sorted_list):
        key = item[0]
        if citations[key].get_citation_count() > max_citation_count / 100:
            most_influential_articles[key] = citations[key]


def get_reference_tree():
    #  Creates list sorted by cumulative number of citations in descending order.
    sorted_list = sorted(citations.items(), key=lambda x: x[1].get_citation_count(), reverse=True)

    result = Node("test")

    for item in list(sorted_list):
        key = item[0]
        result = Node(key)
        cited_by_articles = citations[key].get_cited_by_articles()
        print("key", "=>", result)
        print("cited_articles", "=>", cited_by_articles)

        for article in cited_by_articles:
            Node(article, result)
            print("article", "=>", article)
        break

    return result


def extract_citations_data():
    global processed_article_count, updated

    metadata = ijson.parse(open('../data/internal-citations.json', 'r', encoding='utf8'))
    #   Extracts categories
    articles = ijson.kvitems(metadata, '')

    for article in articles:
        article_id = article[0]
        # This is the list articles which are cited by the given article.
        cited_articles = article[1]
        add_citation(article_id, cited_articles)
        processed_article_count += 1

    # Extracts data generation date.
    # New parser should be created, otherwise another type of objects is not returned.
    metadata = ijson.parse(open('../data/internal-citations.json', 'r', encoding='utf8'))
    updated_items = ijson.items(metadata, 'updated')
    for updated_item in updated_items:
        updated = updated_item


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_citations_data()
    print_summary_data()
    save_only_most_influential_articles()

    reference_tree = get_reference_tree()
    for pre, fill, node in RenderTree(reference_tree):
        print("%s%s" % (pre, node.name))

    #write_output('../data/most-influential-articles.js')
