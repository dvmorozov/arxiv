########################################################################################################################
# File "collect_topics.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

from corpus_iterator import *
from gensim import models
from collections import defaultdict
from gensim import corpora

import logging
logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s', level=logging.INFO)


num_topics: int = 10


def get_path_to_model(model_file_name):
    if len(sys.argv) > 2:
        path_to_model = os.path.join(sys.argv[2], model_file_name)
    else:
        script_path = sys.argv[0]
        path_to_model = os.path.abspath(os.path.join(os.path.dirname(script_path), '../data', model_file_name))

    # print('path to model', path_to_model)
    return path_to_model


def read_model_from_file(file_path, lsi_model):
    global num_topics

    print('Model is read from file', file_path)
    lsi_model.load(file_path)
    print('======================================== Model topics ========================================')
    print(lsi_model.print_topics(num_topics))


def expression_to_js(expression_parts):
    result = '['

    first_word = True
    for part in expression_parts:
        sub_parts = part.split('*')
        assert (len(sub_parts) == 2)
        left = sub_parts[0].strip()
        right = sub_parts[1].strip('" ')

        result += ', ' if not first_word else ''
        result += '{value: "' + left + '",'
        result += ' name: "' + right + '"}'
        first_word = False

    result += ']'
    return result


def write_topics_to_js_file(file_path, model):
    global num_topics
    print('======================================== Model topics ========================================')
    print('Topics are written to JavaScript file', file_path)

    first_topic = True
    topics = 'var flare = {name: "main topics", children: ['
    for topic in model.print_topics(num_topics):
        print(topic)
        topic_index = topic[0]
        topic_expression = topic[1]
        topic_expression_parts = topic_expression.split('+')

        topic_js = ', ' if not first_topic else ''
        topic_js += '{name: "' + str(topic_index)
        topic_js += '", children: ' + expression_to_js(topic_expression_parts)
        topic_js += '}'
        topics += topic_js
        first_topic = False

    topics += ']};'

    print(topics)
    text_file = open(file_path, "w", encoding='utf8')
    text_file.write(topics)
    text_file.close()


def write_model_to_file(file_path, model):
    global num_topics
    print('======================================== Model topics ========================================')
    print(model.print_topics(num_topics))

    print('Model is written to file', file_path)
    model.save(file_path)


def create_model():
    global num_topics

    corpus_iterator = CorpusIterator(get_corpus_directory())

    print('======================================== TF-IDF ========================================')
    tfidf = models.TfidfModel(corpus_iterator)
    tfidf_iterator = tfidf[corpus_iterator]

    # print('========================================= LSI ==========================================')
    # lsi_model = models.LsiModel(tfidf_iterator, id2word=get_dictionary(), num_topics=num_topics)
    # return lsi_model

    print('========================================= LDA ==========================================')
    lda_model = models.LdaMulticore(tfidf_iterator, id2word=get_dictionary(), num_topics=num_topics)
    '''
    Multicore algorithm is crashed on the test dataset.
    lda_model = models.LdaMulticore(corpus=tfidf_iterator,
                             id2word=get_dictionary(),
                             random_state=100,
                             num_topics=num_topics,
                             passes=1,
                             chunksize=1000,
                             batch=False,
                             alpha='asymmetric',
                             decay=0.5,
                             offset=64,
                             eta=None,
                             eval_every=0,
                             iterations=100,
                             gamma_threshold=0.001,
                             per_word_topics=True)
    '''
    return lda_model


def collect_corpus_topic():
    read_dictionary_from_file(get_path_to_dictionary())
    write_topics_to_js_file(get_path_to_model('collected_topics_10.js'), create_model())


if __name__ == '__main__':
    collect_corpus_topic()
    #read_model_from_file(get_path_to_model('model.mm'), create_model())
