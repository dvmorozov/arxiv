########################################################################################################################
# File "bag_of_words.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


from gensim import *
from pprint import pprint  # pretty-printer
from collections import defaultdict


dictionary = corpora.Dictionary()


def read_file(file_path):
    file = open(file_path, 'r')
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
    special_characters = ',:;.≤≥-+=*/?!()[]{}π∇▽∗"\'′−'
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
    print('Original words count', '=>', len(words))

    words = trim(words)
    print('After trimming words', '=>', len(words))

    words = filter_out_stop_words(words)
    print('After filtering out stop words', '=>', len(words))

    words = filter_out_rare_words(words)
    print('After filtering out rare words', '=>', len(words))

    words = filter_out_short_words(words)
    print('After filtering out short words', '=>', len(words))
    return words


def add_to_dictionary(words):
    global dictionary

    # Single document is added as a set of words.
    dictionary.add_documents([words])
    print(dictionary)


def get_bag_of_words_from_file(file_path):
    text = read_file(file_path)
    words = do_preprocessing(text)
    add_to_dictionary(words)
    # print(words)


def write_dictionary_to_file(file_path):
    global dictionary

    dictionary.save_as_text(file_path)


if __name__ == '__main__':
    get_bag_of_words_from_file('0705.1248v1.txt')
    get_bag_of_words_from_file('1202.1776v1.txt')
    write_dictionary_to_file('dictionary.txt')