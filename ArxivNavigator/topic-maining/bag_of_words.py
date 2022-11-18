########################################################################################################################
# File "bag_of_words.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


def get_bag_of_words_from_file(file_path):
    file = open(file_path, 'r')
    data = file.read()
    print(data)


if __name__ == '__main__':
    get_bag_of_words_from_file('/home/dmitry/Downloads/arxiv-data/fulltext/0705.1248v1.txt')
