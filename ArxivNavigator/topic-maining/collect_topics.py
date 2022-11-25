########################################################################################################################
# File "collect_topics.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

from corpus_iterator import *
from gensim import models


num_topics: int = 20


def get_path_to_model():
    model_file_name = 'model.mm'
    if len(sys.argv) > 2:
        return os.path.join(sys.argv[2], model_file_name)
    else:
        script_path = sys.argv[0]
        return os.path.abspath(os.path.join(os.path.dirname(script_path), '../data', model_file_name))


def read_model_from_file(file_path, lsi_model):
    global num_topics

    lsi_model.load(file_path)
    lsi_model.print_topics(num_topics)


def write_model_to_file(file_path, lsi_model):
    global num_topics

    lsi_model.save(file_path)
    print('======================================== Model topics ========================================')
    print(lsi_model)
    lsi_model.print_topics(num_topics)


def create_model():
    global num_topics

    corpus_iterator = CorpusIterator(get_corpus_directory())

    print('======================================== TF-IDF ========================================')
    tfidf = models.TfidfModel(corpus_iterator)
    tfidf_iterator = tfidf[corpus_iterator]

    print('========================================= LSI ==========================================')
    lsi_model = models.LsiModel(tfidf_iterator, id2word=get_dictionary(), num_topics=num_topics)
    return lsi_model


def collect_corpus_topic():
    read_dictionary_from_file(get_path_to_dictionary())
    model = create_model()
    write_model_to_file(get_path_to_model(), model)


if __name__ == '__main__':
    collect_corpus_topic()
