########################################################################################################################
# File "month.py"
# Copyright © Dmitry Morozov 2022
# Class represents single month for aggregating article identifiers.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


from common.common import *
from common.time import *

months = dict()
month_names = ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec']


class Month(object):
    def __init__(self, year, month):
        self.year = year
        self.month = month
        self.article_ids = []

    def add_article_id(self, article_id):
        if article_id not in self.article_ids:
            self.article_ids.append(article_id)

    def get_article_ids(self):
        return self.article_ids

    def get_year(self):
        return self.year

    def get_month(self):
        return self.month


def create_months():
    global months, month_names

    months.clear()
    for year in get_years_range():
        for month in month_names:
            month_name = month + '_' + str(year)
            months[month_name] = Month(year, month)


def get_month(year, month_number):
    global month_names

    assert(len(month_names) > 0)
    assert(0 < month_number <= len(month_names))

    month = month_names[month_number - 1]
    month_name = month + '_' + str(year)
    return months[month_name]


def write_months_to_json(file_path):
    months_json = '{"months": ['
    first_month = True

    for month_name in months.keys():
        month = months[month_name]

        if not first_month:
            months_json += ', '

        month_json = '{"year": "' + str(month.get_year()) + '", "month": "' + month.get_month() + '", "article_ids": ['

        article_ids = month.get_article_ids()
        first_article_id = True

        for article_id in article_ids:
            if not first_article_id:
                month_json += ', '
            month_json += '"' + article_id + '"'
            first_article_id = False

        month_json += ']}'

        months_json += month_json
        first_month = False

    months_json += '];'

    write_text_to_file(file_path, months_json, 'utf-8')
