########################################################################################################################
# File "make_corpus_file.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

from collect_dictionary import *


def make_line_from_file(file_path):
    text = read_file(file_path)
    return text.replace('\n', ' ')


def get_corpus_file():
    assert (len(sys.argv) > 2)

    return sys.argv[2]


def make_corpus_file():
    print('Making corpus file...')

    path_to_corpus = get_corpus_directory()
    print('Path to corpus', path_to_corpus)

    path_to_corpus_file = get_corpus_file()
    print('Path to corpus file', path_to_corpus_file)

    corpus_file = open(path_to_corpus_file, "w", encoding='utf8')

    processed_files_count = 0
    text_file_list = get_text_file_list(path_to_corpus)
    estimated_time = EstimatedTime(len(text_file_list), 'Making corpus file')

    while len(text_file_list) > 0:
        file_name = text_file_list.pop()
        path_to_text = os.path.join(path_to_corpus, file_name)

        if os.path.isfile(path_to_text):
            line = make_line_from_file(path_to_text) + '\n'
            corpus_file.write(line)

            processed_files_count += 1
            estimated_time.print_estimate_time(processed_files_count)

    corpus_file.close()


if __name__ == '__main__':
    make_corpus_file()
