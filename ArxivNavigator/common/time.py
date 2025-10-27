########################################################################################################################
# File "time.py"
# Copyright © Dmitry Morozov 2023
# Module contains generic functions to work with time intervals.
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


import datetime


def get_years_range():
    current_date = datetime.datetime.now().date()
    current_year = int(current_date.strftime("%Y"))
    return range(1985, current_year + 1)
