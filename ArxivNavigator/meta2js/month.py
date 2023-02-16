########################################################################################################################
# File "month.py"
# Copyright © Dmitry Morozov 2022
# Class represents single month for aggregating article identifiers.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


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
