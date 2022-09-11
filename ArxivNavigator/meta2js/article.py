########################################################################################################################
# File "article.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


class Article(object):
    def __init__(self, article_id, title, last_version_date):
        self.id = article_id
        # Saves only alphanumeric characters in the title.
        self.title = ''.join(c for c in title if c.isalnum() or c in [' ', ',', '.', '-'])
        self.title = self.title.replace('  ', ' ')
        self.last_version_date = last_version_date

    def get_last_version_date(self):
        return self.last_version_date

    def get_title(self):
        return self.title

    def get_json(self) -> str:
        return '{id: "' + self.id + '", ' \
                'title: "' + self.title + '", ' \
                'last_version_date: "' + str(self.last_version_date) + '"}'
