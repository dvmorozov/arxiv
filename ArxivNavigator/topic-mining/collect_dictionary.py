########################################################################################################################
# File "collect_dictionary.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
# Script parameters:
#   N1 - path to corpus directory,
#   N2 - path to dictionary,
#   N3 - corpus encoding.
########################################################################################################################


import os
import sys
from collections import defaultdict

import nltk
from gensim import *
from nltk.stem.wordnet import WordNetLemmatizer

from common.estimated_time import *

nltk.download('wordnet')
nltk.download('omw-1.4')

# The dictionary represents vector space.
dictionary = corpora.Dictionary()
lemmatizer = WordNetLemmatizer()


def get_dictionary():
    return dictionary


def read_file(file_path):
    file = open(file_path, 'r', encoding=get_corpus_encoding())
    file_content = file.read()
    file.close()
    return file_content


def filter_out_stop_words(words):
    # Removes common words and tokenizes.
    stop_list = set('for a of the that and to in is are'.split()).union(parsing.preprocessing.STOPWORDS)
    return [word for word in words if word not in stop_list]


def contains(string, characters):
    for c in characters:
        if string.rfind(c) != -1:
            return True

    return False


def trim(words):
    # TODO: Move special characters into corpus-specific module.
    special_characters = ',:;.≤≥-+=*/?!()[]{}⟨⟩<>πλμηχψτω̄∇▽∗"\'′−\x01'
    for i in range(1, len(words)):
        words[i] = words[i].strip(special_characters)

    # Removes empty words. Saves only words containing alphabetical characters.
    words = [word for word in words if len(word) > 0 and word.isalpha()]

    return words


def lemmatize_words(words):
    words = [lemmatizer.lemmatize(word) for word in words]
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
    global lemmatizer

    words = text.lower().split()
    # print('Original words count', '=>', len(words))

    words = trim(words)
    # print('After trimming words', '=>', len(words))

    words = lemmatize_words(words)
    # print('Lemmatized words count', '=>', len(words))

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

    print('Dictionary having', str(len(dictionary)), 'items is saved into', file_path, '.')
    dictionary.save_as_text(file_path)


def read_dictionary_from_file(file_path):
    global dictionary

    dictionary = dictionary.load_from_text(file_path)
    print(dictionary)


def get_corpus_directory():
    assert (len(sys.argv) > 1)

    result = sys.argv[1]

    print('Corpus directory is', result, '.')
    return result


def get_path_to_dictionary():
    dictionary_file_name = 'dictionary.txt'
    if len(sys.argv) > 2:
        result = os.path.join(sys.argv[2], dictionary_file_name)
    else:
        script_path = sys.argv[0]
        result = os.path.abspath(os.path.join(os.path.dirname(script_path), '../data', dictionary_file_name))

    print('Path to dictionary is', result, '.')
    return result


def get_corpus_encoding():
    if len(sys.argv) > 3:
        result = sys.argv[3]
    else:
        result = 'utf8'

    return result


def get_metadata_path(file_path):
    file_path_parts = os.path.splitext(file_path)
    assert(len(file_path_parts) > 0)
    return file_path_parts[0] + '.ini'


def is_ext_equal(file_path, ext):
    file_path_parts = os.path.splitext(file_path)
    return len(file_path_parts) > 0 and file_path_parts[1] == ext


def get_text_file_list(path_to_texts):
    dir_list = os.listdir(path_to_texts)
    result = []

    for file_name in dir_list:
        path_to_text = os.path.join(path_to_texts, file_name)
        if is_ext_equal(path_to_text, '.txt'):
            path_to_meta = get_metadata_path(path_to_text)
            if not os.path.exists(path_to_meta):
                result.append(path_to_text)

    print('Number of files to process', str(len(result)))
    return result


def collect_corpus_dictionary():
    print('Collection corpus dictionary...')

    path_to_texts = get_corpus_directory()

    processed_files_count = 0
    text_file_list = get_text_file_list(path_to_texts)
    estimated_time = EstimatedTime(len(text_file_list), 'Collecting dictionary')

    while len(text_file_list) > 0:
        file_name = text_file_list.pop()
        path_to_text = os.path.join(path_to_texts, file_name)
        path_to_meta = get_metadata_path(path_to_text)

        if not os.path.exists(path_to_meta):
            if os.path.isfile(path_to_text):
                get_bag_of_words_from_file(path_to_text)
                processed_files_count += 1
                estimated_time.print_estimate_time(processed_files_count)

                # Remove this to proceed.
                if processed_files_count % 1000 == 0:
                    write_dictionary_to_file(get_path_to_dictionary())

    write_dictionary_to_file(get_path_to_dictionary())


def file_to_bow(file_path):
    # print('File', file_path, 'is processed.')
    text = read_file(file_path)
    words = do_preprocessing(text)
    vector = dictionary.doc2bow(words)
    return vector


if __name__ == '__main__':
    collect_corpus_dictionary()
