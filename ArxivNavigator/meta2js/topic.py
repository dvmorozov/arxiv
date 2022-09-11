########################################################################################################################
# File "topic.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################
import datetime


class Topic(object):
    def __init__(self, topic_id, article_count):
        self.article_count = article_count
        #   The first part of topic id. is taken as group id.
        self.group = topic_id.split('.')[0]
        self.id = topic_id
        self.last_articles = []
        self.max_last_articles_count = 10

    def inc_article_count(self):
        self.article_count += 1

    def get_last_articles_json(self) -> str:
        # Articles are sorted in time-reverse order.
        sorted_articles = sorted(self.last_articles, key=lambda x: datetime.datetime.now() - x.get_last_version_date())

        article_count = 0
        result: str = 'last_articles: ['

        for article in sorted_articles:
            if article_count != 0:
                result += ','
            result += article.get_json()
            article_count += 1

        result += ']'
        return result

    def get_json(self) -> str:
        return '{id: "' + str(self.id) + '", ' \
                'value: ' + str(self.article_count) + ', ' \
                'group: "' + self.group + '", ' + \
                self.get_last_articles_json() + '}'

    def get_first_article_date(self):
        if len(self.last_articles) == 0:
            return None

        first_article = min(self.last_articles, key=lambda article: article.get_last_version_date())
        return first_article.get_last_version_date()

    def get_first_article_index(self):
        if len(self.last_articles) == 0:
            return None

        first_article = min(self.last_articles, key=lambda article: article.get_last_version_date())
        return self.last_articles.index(first_article)

    def add_to_last_articles(self, article):
        if self.get_first_article_date() is None or \
                article.get_last_version_date() > self.get_first_article_date():
            if len(self.last_articles) >= self.max_last_articles_count:
                first_article_index = self.get_first_article_index()

                if first_article_index is not None:
                    del self.last_articles[first_article_index]

            self.last_articles.append(article)


unique_topics = dict()


def add_unique_topic(topic_id):
    global unique_topics

    if topic_id not in unique_topics:
        unique_topics[topic_id] = Topic(topic_id, 1)
    else:
        unique_topics[topic_id].inc_article_count()

    return unique_topics[topic_id]


def generate_topics_json() -> str:
    global unique_topics

    topics_json: str = 'nodes: ['
    topics_count = 0

    for node in unique_topics:
        if topics_count != 0:
            topics_json += ','

        topic = unique_topics[node]

        topics_json += topic.get_json()
        topics_count += 1

    topics_json += ']'
    return topics_json


def get_topic_count():
    return len(unique_topics)
