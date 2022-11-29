########################################################################################################################
# File "estimated_time.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


import datetime


class EstimatedTime(object):
    def __init__(self, item_count, caption):
        self.item_count = item_count
        self.started_time = datetime.datetime.now()
        self.caption = caption

    # This method must be called on every item.
    def print_estimate_time(self, item_processed):
        if item_processed <= self.item_count:
            item_step = int(self.item_count / 100)
            if item_processed % item_step == 0:
                processed_percents: float = item_processed * 100.0 / self.item_count
                elapsed_sec = (datetime.datetime.now() - self.started_time).total_seconds()
                # print('elapsed_sec', elapsed_sec, 'item_processed', item_processed, 'item_count', item_count)
                estimated_sec = datetime.timedelta(
                    seconds=elapsed_sec * (self.item_count - item_processed) / item_processed)
                estimated_time = datetime.datetime(1, 1, 1) + estimated_sec
                print(self.caption, '%.2f' % processed_percents, '%, estimated time=',
                      "%d:%d:%d:%d" % (
                          estimated_time.day - 1, estimated_time.hour, estimated_time.minute, estimated_time.second),
                      ', processed', str(item_processed), '.')
        else:
            print(self.caption, 'finished. Item number', str(item_processed), '.')
