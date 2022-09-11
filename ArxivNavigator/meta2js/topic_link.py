########################################################################################################################
# File "topic_link.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################

from topic import *


class TopicLink(object):
    def __init__(self, source_id, target_id, link_multiplicity):
        self.link_multiplicity = link_multiplicity
        self.source_id = source_id
        self.target_id = target_id

    def inc_multiplicity(self):
        self.link_multiplicity += 1

    def get_multiplicity(self):
        return self.link_multiplicity

    def get_json(self):
        return '{source: "' + self.source_id + '", ' \
                'target: "' + self.target_id + '", ' \
                'value: ' + str(self.link_multiplicity) + '}'


#   Set of tuples (source, target)
unique_links = dict()
link_count = 0


def add_unique_link(source_topic_id, target_topic_id):
    global unique_links

    #   Link has no direction.
    if ((source_topic_id, target_topic_id) not in unique_links) and \
       ((target_topic_id, source_topic_id) not in unique_links):
        #   Link is added for the first time.
        unique_links[source_topic_id, target_topic_id] = TopicLink(source_topic_id, target_topic_id, 1)
    else:
        #   Value is incremented.
        if (source_topic_id, target_topic_id) in unique_links:
            unique_links[source_topic_id, target_topic_id].inc_multiplicity()
        else:
            #   Key values have been added in reverse order for the first time.
            unique_links[target_topic_id, source_topic_id].inc_multiplicity()


def generate_links_json() -> str:
    global unique_links, link_count

    links_json: str = 'links: ['
    link_count = 0

    #  Creates list sorted by multiplicity in ascending order.
    sorted_list = sorted(unique_links.items(), key=lambda x: x[1].get_multiplicity())

    #  Creates sorted dictionary.
    sorted_links = dict(sorted_list)

    #  Removes "weak" links (having small values) but keeps at least one existing link between any two nodes.
    for key in list(sorted_links):
        #  Checks lower bound on acceptable number of links.
        if len(sorted_links) <= len(unique_topics):
            break

        source = key[0]
        target = key[1]

        #   Checks if related nodes have at least one more links.
        source_found = False
        target_found = False
        for key2 in list(sorted_links):
            if key2 == key:
                continue

            source2 = key2[0]
            target2 = key2[1]
            if source2 == source:
                source_found = True
            if target2 == target:
                target_found = True
            if source_found and target_found:
                break

        #   Deletes the link.
        if source_found and target_found:
            del sorted_links[key]

    for key in sorted_links:
        topic_link = sorted_links[key]

        if link_count != 0:
            links_json += ','
        links_json += topic_link.get_json()
        link_count += 1

    links_json += ']'
    return links_json


def get_link_count():
    global link_count
    return link_count
