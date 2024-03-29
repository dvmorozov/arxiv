########################################################################################################################
# File "collect_topics_by_months.py"
# Copyright © Dmitry Morozov 2023
# If you want to use this file please contact me by dvmorozov@hotmail.com.
# Script parameters:
#   N1 - path to corpus directory,
#   N2 - temporary directory,
#   N3 - corpus encoding.
########################################################################################################################


import os.path
import shutil
from meta2js.month import *
from collect_topics import *
from common.log import *


copy_log = Log("")


def get_temporary_directory():
    assert (len(sys.argv) > 2)

    result = sys.argv[2]
    print('Temporary directory is {0}.'.format(result))
    return result


def remove_text_files_from_directory(directory_path):
    write_log_to_file(get_work_log(), 'Directory {0} is cleaned.'.format(directory_path))

    if not os.path.exists(directory_path):
        os.makedirs(directory_path, exist_ok=True)

    files_to_delete = get_text_file_list(directory_path)
    for file_path in files_to_delete:
        os.remove(file_path)


def get_file_name_from_article_id(article_id):
    name = article_id[0].split('/')
    name = name[1] if len(name) > 1 else name[0]
    version = article_id[1]
    return name + version + '.txt'


def copy_articles_into_directory(directory_path, article_ids):
    global copy_log

    corpus_directory = get_corpus_directory()
    copied_files_count = 0
    not_existing_files_count = 0

    for article_id in article_ids:
        file_name = get_file_name_from_article_id(article_id)
        src_path = os.path.join(corpus_directory, file_name)
        dst_path = os.path.join(directory_path, file_name)
        if os.path.exists(src_path):
            write_log_to_file(copy_log, 'File {0} is copied to {1}.'.format(src_path, directory_path))
            shutil.copyfile(src_path, dst_path)
            copied_files_count += 1
        else:
            write_log_to_file(copy_log, 'File {0} does not exits. Article id. is {1}.'.format(src_path, article_id))
            not_existing_files_count += 1

    return copied_files_count, not_existing_files_count


def mine_topics_month_by_month():
    global copy_log

    temporary_directory = get_temporary_directory()
    corpus_directory = os.path.join(temporary_directory, 'corpus')
    corpus_encoding = get_corpus_encoding()
    path_to_dictionary = os.path.join(temporary_directory, 'dictionary.txt')
    path_to_copy_log = os.path.join(temporary_directory, 'copy.log.txt')
    path_to_work_log = os.path.join(temporary_directory, 'work.log.txt')
    path_to_topic_by_months_js = "../data/topic_by_months.js"

    copy_log = Log(path_to_copy_log)
    init_work_log(path_to_work_log)

    clear_months()
    read_months_from_json('../data/months.json')

    for month_name in months.keys():
        month = months[month_name]
        write_log_to_file(get_work_log(),
                          '======================================== Month {0} ========================================'.
                          format(month_name))

        article_ids = month.get_article_ids()
        if len(article_ids) == 0:
            continue

        # Copy articles into temporary directory to mine by external script.
        remove_text_files_from_directory(corpus_directory)
        copied_files_count, not_existing_files_count = copy_articles_into_directory(corpus_directory, article_ids)
        if copied_files_count == 0:
            continue

        write_log_to_file(get_work_log(), 'Number of not existing files {0} for month {1} .'.format(
            not_existing_files_count, month_name))

        #  It's Ok to collect dictionary for a bunch of files for every month.
        collect_corpus_dictionary(corpus_directory, path_to_dictionary, corpus_encoding)

        month.set_topics(collect_corpus_topic(corpus_directory, path_to_dictionary, corpus_encoding, 1))

    write_month_topics_to_js(path_to_topic_by_months_js)


if __name__ == '__main__':
    mine_topics_month_by_month()
