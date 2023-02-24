########################################################################################################################
# File "corpus_iterator.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


import os
from collect_dictionary import *
from common.estimated_time import *


class CorpusIterator(object):
    def __init__(self, corpus_directory, corpus_encoding):
        self.corpus_directory = corpus_directory
        self.corpus_encoding = corpus_encoding
        self.dir_list = os.listdir(self.corpus_directory)
        self.estimated_time = EstimatedTime(len(self.dir_list), 'Processing corpus')

    def __iter__(self):
        self.processed_files_count = 0
        for file_name in self.dir_list:
            path_to_text = os.path.join(self.corpus_directory, file_name)

            self.processed_files_count += 1
            self.estimated_time.print_estimate_time(self.processed_files_count)

            if os.path.exists(path_to_text) and is_ext_equal(path_to_text, '.txt'):
                bow = file_to_bow(path_to_text, self.corpus_encoding)
            else:
                print('empty bow is returned')
                bow = []

            # print('bow', bow)
            yield bow
