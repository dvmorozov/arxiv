########################################################################################################################
# File "collect_topics.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
# Script parameters:
#   N1 - path to corpus directory,
#   N2 - path to dictionary,
#   N3 - corpus encoding,
#   N4 - path to model.
########################################################################################################################


from corpus_iterator import *
from common.log import *


logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s', level=logging.INFO)


def get_path_to_model(model_file_name):
    if len(sys.argv) > 4:
        path_to_model = os.path.join(sys.argv[4], model_file_name)
    else:
        script_path = sys.argv[0]
        path_to_model = os.path.abspath(os.path.join(os.path.dirname(script_path), '../data', model_file_name))

    write_log_to_file(get_work_log(), 'Path to model is {0}.'.format(path_to_model))
    return path_to_model


def read_model_from_file(file_path, lsi_model, num_topics):
    write_log_to_file(get_work_log(), 'Model is read from file {0}.'.format(file_path))
    lsi_model.load(file_path)
    write_log_to_file(get_work_log(),
                      '======================================== Model topics ========================================')
    write_log_to_file(get_work_log(), lsi_model.print_topics(num_topics))


def topic_items_to_js(topic_items):
    result = '['

    first_word = True
    for item in topic_items:
        assert(len(item) == 2)

        result += ', ' if not first_word else ''
        result += '{value: "' + item[1] + '",'
        result += ' name: "' + item[0] + '"}'
        first_word = False

    result += ']'
    return result


def expression_to_array(topic_expression):
    result = []

    expression_parts = topic_expression.split('+')

    for part in expression_parts:
        sub_parts = part.split('*')
        assert (len(sub_parts) == 2)

        value = sub_parts[0].strip()
        name = sub_parts[1].strip('" ')
        result.append((name, value))

    return result


def get_topics_from_model(model, num_topics):
    write_log_to_file(get_work_log(),
                      '======================================== Model topics ========================================')

    result = []
    for topic in model.print_topics(num_topics):
        write_log_to_file(get_work_log(), 'Topic: {0}.'.format(topic))

        topic_name = str(topic[0])
        topic_expression = topic[1]

        topic_items = expression_to_array(topic_expression)
        result.append((topic_name, topic_items))

    return result


def write_topics_to_js_file(file_path, topics):
    write_log_to_file(get_work_log(),
                      '======================================== Model topics ========================================')
    write_log_to_file(get_work_log(), 'Topics are written to JavaScript file {0}.'.format(file_path))

    first_topic = True
    topics_js = 'var flare = {name: "main topics", children: ['
    for topic in topics:
        assert(len(topic) == 2)

        topic_name = topic[0]
        topic_items = topic[1]

        topic_js = ', ' if not first_topic else ''
        topic_js += '{name: "' + topic_name
        topic_js += '", children: ' + topic_items_to_js(topic_items)
        topic_js += '}'
        topics_js += topic_js
        first_topic = False

    topics_js += ']};'

    write_log_to_file(get_work_log(), 'Topics: {0}.'.format(topics_js))
    text_file = open(file_path, "w", encoding='utf8')
    text_file.write(topics_js)
    text_file.close()


def write_model_to_file(file_path, model, num_topics):
    write_log_to_file(get_work_log(),
                      '======================================== Model topics '
                      '===========================================')
    write_log_to_file(get_work_log(), model.print_topics(num_topics))
    write_log_to_file(get_work_log(), 'Model is written to file {0}.'.format(file_path))
    model.save(file_path)


def create_model(corpus_directory, corpus_encoding, num_topics):
    corpus_iterator = CorpusIterator(corpus_directory, corpus_encoding)

    # print('======================================== TF-IDF ========================================')
    # tfidf = models.TfidfModel(corpus_iterator)
    # tfidf_iterator = tfidf[corpus_iterator]

    # print('========================================= LSI ==========================================')
    # lsi_model = models.LsiModel(tfidf_iterator, id2word=get_dictionary(), num_topics=num_topics)
    # return lsi_model

    write_log_to_file(get_work_log(),
                      '======================================== LDA '
                      '===================================================')
    lda_model = models.LdaMulticore(corpus_iterator, id2word=get_dictionary(), num_topics=num_topics)
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


def collect_corpus_topic(corpus_directory, path_to_dictionary, corpus_encoding, num_topics):
    read_dictionary_from_file(path_to_dictionary)
    model = create_model(corpus_directory, corpus_encoding, num_topics)
    return get_topics_from_model(model, num_topics)


if __name__ == '__main__':
    write_topics_to_js_file(get_path_to_model('collected_topics_10.js'),
                            collect_corpus_topic(get_corpus_directory(),
                                                 get_path_to_dictionary(),
                                                 get_corpus_encoding(), 10))
