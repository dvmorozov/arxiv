########################################################################################################################
# File "estimated_time.py"
# Copyright © Dmitry Morozov 2023
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


class Log(object):
    def __init__(self, path_to_log_file):
        self.path_to_log_file = path_to_log_file

    def write_log_to_file(self, text):
        textfile = open(self.path_to_log_file, "a", encoding="utf-8")
        textfile.write(text)
        textfile.write('\n')
        textfile.close()
