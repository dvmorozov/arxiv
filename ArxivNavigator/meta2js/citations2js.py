########################################################################################################################
# File "citations2js.py"
# Copyright © Dmitry Morozov 2022
# This script extracts most influential articles from citation data.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import ijson
from datetime import datetime

article_count = 0
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
        article_citations = article[1]
        print(article_id)
        print(article_citations)

        article_count += 1

    '''
    # New parser should be created, otherwise another type of objects is not returned.
    metadata = ijson.parse(open('../data/internal-citations.json', 'r'))
    updated_items = ijson.items(metadata, 'updated')
    for updated_item in updated_items:
        updated = updated_item

    finish_parsing('../data/most-influential-articles.js')
    '''
    print(str(article_count))


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_articles_data()
