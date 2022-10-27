########################################################################################################################
# File "citations2js.py"
# Copyright © Dmitry Morozov 2022
# This script extracts most influential articles from citation data.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import ijson
from citation import *

updated = ""


def finish_parsing(write_to_file):
    global updated

    print('articles', '=>', str(article_count))
    print('updated', '=>', str(updated))

    topics = 'var topics = {' + \
        'article_count: "' + str(article_count) + '", ' \
        'updated: "' + str(updated) + '"};'

    textfile = open(write_to_file, "w")
    textfile.write(topics)
    textfile.close()


def extract_articles_data():
    global article_count, updated

    metadata = ijson.parse(open('../data/internal-citations.json', 'r'))
    #   Extracts categories
    articles = ijson.kvitems(metadata, '')

    for article in articles:
        article_id = article[0]
        # This is the list articles which are cited by the given article.
        cited_articles = article[1]
        add_citation(article_id, cited_articles)

    #  Creates list sorted by cumulative number of citations in descending order.
    sorted_list = sorted(citations.items(), key=lambda x: x[1].get_citation_count(), reverse=True)

    max_citation_count = 0
    if len(sorted_list) > 0:
        key = sorted_list[0][0]
        max_citation_count = citations[key].get_citation_count()

    print('max citation count', '=>', max_citation_count)

    for item in list(sorted_list):
        key = item[0]
        if citations[key].get_citation_count() > max_citation_count / 100:
            print(citations[key].get_article_id(), '=>', str(citations[key].get_citation_count()))

    print(str(get_citations_count()))

    '''
    # New parser should be created, otherwise another type of objects is not returned.
    metadata = ijson.parse(open('../data/internal-citations.json', 'r'))
    updated_items = ijson.items(metadata, 'updated')
    for updated_item in updated_items:
        updated = updated_item

    finish_parsing('../data/most-influential-articles.js')
    '''


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_articles_data()
