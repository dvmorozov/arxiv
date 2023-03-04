########################################################################################################################
# File "estimated_time.py"
# Copyright © Dmitry Morozov 2023
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


import os.path


class Log(object):
    def __init__(self, path_to_log_file):
        self.path_to_log_file = path_to_log_file
        if self.path_to_log_file != "" and os.path.exists(self.path_to_log_file):
            os.remove(self.path_to_log_file)

    def write_log_to_file(self, text):
        assert(self.path_to_log_file != "")

        textfile = open(self.path_to_log_file, "a", encoding="utf-8")
        textfile.write(text)
        textfile.write('\n')
        textfile.close()


def write_log_to_file(log, text):
    assert (log is not None)
    log.write_log_to_file(text)


work_log = Log("")


def init_work_log(path_to_work_log):
    global work_log

    work_log = Log(path_to_work_log)


def get_work_log():
    return work_log
