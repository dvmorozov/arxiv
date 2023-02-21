########################################################################################################################
# File "collect_topics_by_months.py"
# Copyright © Dmitry Morozov 2023
# If you want to use this file please contact me by dvmorozov@hotmail.com.
# Script parameters:
#   N1 - path to corpus directory,
#   N2 - corpus encoding,
#   N3 - temporary directory.
########################################################################################################################


import os.path
import shutil
from meta2js.month import *
from collect_dictionary import *


def get_temporary_directory():
    assert (len(sys.argv) > 3)

    result = sys.argv[3]
    print('Temporary directory is', result)
    return result


def clean_temporary_directory(directory_path):
    print('Directory', directory_path, 'is cleaned.')

    if not os.path.exists(directory_path):
        os.makedirs(directory_path, exist_ok=True)

    files_to_delete = get_text_file_list(directory_path)
    for file_path in files_to_delete:
        print('File', file_path, 'is removed.')
        os.remove(file_path)


def get_file_name_from_article_id(article_id):
    return article_id + '.txt'


def copy_articles_into_temporary_directory(directory_path, article_ids):
    corpus_directory = get_corpus_directory()
    print('Articles are copied from', corpus_directory, 'to', directory_path, '.')

    for article_id in article_ids:
        file_name = get_file_name_from_article_id(article_id)
        src_path = os.path.join(corpus_directory, file_name)
        dst_path = os.path.join(directory_path, file_name)
        if os.path.exists(src_path):
            shutil.copyfile(src_path, dst_path)
        else:
            print('File', src_path, 'does not exits.')


def mine_topics_month_by_month():
    temporary_directory = get_temporary_directory()

    clear_months()
    read_months_from_json('../data/months.json')

    for month_name in months.keys():
        month = months[month_name]
        article_ids = month.get_article_ids()
        if len(article_ids) == 0:
            continue

        # Copy articles into temporary directory to mine by external script.
        clean_temporary_directory(temporary_directory)
        copy_articles_into_temporary_directory(temporary_directory, article_ids)

        # Collect dictionary.

        # Collect topics.

        # Read topics and write to the output structure.

        # Write output structure to file.

        # TODO: remove this.
        # break


if __name__ == '__main__':
    mine_topics_month_by_month()
