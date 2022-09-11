########################################################################################################################
# File "topic.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


class Topic(object):
    article_count = 0
    group = ""
    id = ""

    def __init__(self, topic_id, article_count):
        self.article_count = article_count
        #   The first part of topic id. is taken as group id.
        self.group = topic_id.split('.')[0]
        self.id = topic_id

    def inc_article_count(self):
        self.article_count += 1

    def get_topic_json(self) -> str:
        return '{id: "' + str(self.id) + '", value: ' + str(self.article_count) + ', group: "' + self.group + '"}'


unique_topics = dict()


def add_unique_topic(topic_id):
    global unique_topics

    if topic_id not in unique_topics:
        unique_topics[topic_id] = Topic(topic_id, 1)
    else:
        unique_topics[topic_id].inc_article_count()


def generate_topics_json() -> str:
    global unique_topics

    topics_json: str = 'nodes: ['
    topics_count = 0

    for node in unique_topics:
        if topics_count != 0:
            topics_json += ','

        topic = unique_topics[node]

        topics_json += topic.get_topic_json()
        topics_count += 1

    topics_json += ']'
    return topics_json


def get_topic_count():
    return len(unique_topics)
