########################################################################################################################
# File "bag_of_words.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


import os
import sys
from gensim import *
from pprint import pprint  # pretty-printer
from collections import defaultdict
from common.estimated_time import *


# The dictionary represents vector space.
dictionary = corpora.Dictionary()


def read_file(file_path):
    file = open(file_path, 'r', encoding='utf8')
    return file.read()


def filter_out_stop_words(words):
    # Removes common words and tokenizes.
    stop_list = set('for a of the and to in'.split()).union(parsing.preprocessing.STOPWORDS)
    return [word for word in words if word not in stop_list]


def contains(string, characters):
    for c in characters:
        if string.rfind(c) != -1:
            return True

    return False


def trim(words):
    special_characters = ',:;.≤≥-+=*/?!()[]{}⟨⟩<>πλμηχψτω̄∇▽∗"\'′−'
    for i in range(1, len(words)):
        words[i] = words[i].strip(special_characters)

    # Removes empty words.
    words = [word for word in words if len(word) > 0]

    # Removes words containing special characters in the middle.
    words = [word for word in words if not contains(word, special_characters)]

    return words


def filter_out_rare_words(words):
    frequency = defaultdict(int)

    for word in words:
        frequency[word] += 1

    # Removes words that appear only once.
    return [word for word in words if frequency[word] > 1]


def filter_out_short_words(words):
    return [word for word in words if len(word) > 3]


def do_preprocessing(text):
    words = text.lower().split()
    # print('Original words count', '=>', len(words))

    words = trim(words)
    # print('After trimming words', '=>', len(words))

    words = filter_out_stop_words(words)
    # print('After filtering out stop words', '=>', len(words))

    words = filter_out_rare_words(words)
    # print('After filtering out rare words', '=>', len(words))

    words = filter_out_short_words(words)
    # print('After filtering out short words', '=>', len(words))
    return words


def add_to_dictionary(words):
    global dictionary

    # Single document is added as a set of words.
    dictionary.add_documents([words])


def get_bag_of_words_from_file(file_path):
    text = read_file(file_path)
    words = do_preprocessing(text)
    add_to_dictionary(words)
    # print(words)


def write_dictionary_to_file(file_path):
    global dictionary

    print(dictionary)
    dictionary.save_as_text(file_path)


def read_dictionary_from_file(file_path):
    global dictionary

    dictionary = dictionary.load_from_text(file_path)
    print(dictionary)


def get_corpus_directory():
    assert (len(sys.argv) > 1)

    return sys.argv[1]


def get_corpus_dictionary():
    print('Collection corpus dictionary...')

    path_to_texts = get_corpus_directory()
    print('Corpus directory', path_to_texts)

    path_to_dictionary = os.path.join(get_corpus_directory(), 'dictionary.txt')

    processed_files_count = 0
    dir_list = os.listdir(path_to_texts)
    estimated_time = EstimatedTime(len(dir_list), 'Collecting dictionary')

    for filename in dir_list:
        path_to_text = os.path.join(path_to_texts, filename)

        if os.path.isfile(path_to_text):
            get_bag_of_words_from_file(path_to_text)
            processed_files_count += 1
            estimated_time.print_estimate_time(processed_files_count)

            # Remove this to proceed.
            if processed_files_count == 10:
                break

    write_dictionary_to_file(path_to_dictionary)


def file_to_vector(file_path):
    text = read_file(file_path)
    words = do_preprocessing(text)
    vector = dictionary.doc2bow(words)
    print(vector)
    return vector


if __name__ == '__main__':
    get_corpus_dictionary()
    # read_dictionary_from_file(path_to_dictionary)

    # Displays token ids.
    #pprint(dictionary.token2id)

    file_to_vector(os.path.join(get_corpus_directory(), "1205.3815v1.txt"))
