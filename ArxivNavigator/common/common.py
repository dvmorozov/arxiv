########################################################################################################################
# File "common.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

import os
import time
import random
import sys


def get_corpus_encoding():
    if len(sys.argv) > 3:
        result = sys.argv[3]
    else:
        result = 'utf8'

    return result


def write_text_to_file(file_path, text, encoding):
    dir_name = os.path.dirname(file_path)
    if not os.path.exists(dir_name):
        os.makedirs(dir_name)

    textfile = open(file_path, "w", encoding=encoding)
    textfile.write(text)
    textfile.close()


def random_sleep(sec=500.0):
    sleep_secs = sec + (sec / 5.0) * random.random()
    print('Sleep', str(sleep_secs), 'secs')
    time.sleep(sleep_secs)


def get_script_parameter(parameter_index):
    assert (len(sys.argv) > parameter_index)

    result = sys.argv[parameter_index]
    return result


def get_domain():
    return get_script_parameter(1)


def get_data_dir():
    return get_script_parameter(2)


def get_domain_directory_name():
    return get_domain().replace('.', '_')


def get_domain_dir():
    return os.path.join(get_data_dir(), get_domain_directory_name())


def check_and_create_domain_dir():
    old_path = os.path.join(get_data_dir(), get_domain())
    new_path = get_domain_dir()

    if os.path.exists(old_path) and not os.path.exists(new_path):
        print('Directory', old_path, 'is renamed into', new_path, '.')
        os.rename(old_path, new_path)
        return

    if os.path.exists(new_path):
        print('Directory', new_path, 'exists.')
        if os.path.exists(old_path):
            os.rmdir(old_path)
            print('Directory', old_path, 'has been removed.')
        return

    print('Directory', new_path, 'is created.')
    os.makedirs(new_path, exist_ok=False)
