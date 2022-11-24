########################################################################################################################
# File "collect_topics.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

from corpus_iterator import *
from gensim import models

if __name__ == '__main__':
    read_dictionary_from_file(get_path_to_dictionary())

    corpus_iterator = CorpusIterator(get_corpus_directory())
    tfidf = models.TfidfModel(corpus_iterator)
    for vector in tfidf:
        print(vector)


