########################################################################################################################
# File "corpus_iterator.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import os
from collect_dictionary import *


class CorpusIterator(object):
    def __init__(self, corpus_path):
        self.corpus_path = corpus_path

    def __iter__(self):
        dir_list = os.listdir(self.corpus_path)

        for file_name in dir_list:
            path_to_text = os.path.join(self.corpus_path, file_name)
            if os.path.exists(path_to_text) and is_ext_equal(path_to_text, '.txt'):
                bow = file_to_vector(path_to_text)
                print(bow)
                yield bow
