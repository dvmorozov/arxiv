# This script converts downloaded arxiv metadata into graph data suitable for 3D.js.
import ijson


#   Set of tuples (source, target)
unique_links = dict()
unique_nodes = dict()
link_count = 0
article_count = 0


def add_node(node):
    global unique_nodes

    if node not in unique_nodes:
        unique_nodes[node] = 1
    else:
        unique_nodes[node] += 1


def add_link(source, target):
    global unique_links

    #   Link has no direction.
    if ((source, target) not in unique_links) and ((target, source) not in unique_links):
        #   Link is added for the first time.
        unique_links[(source, target)] = 1
    else:
        #   Value is incremented.
        if (source, target) in unique_links:
            unique_links[(source, target)] += 1
        else:
            #   Key values have been added in reverse order for the first time.
            unique_links[(target, source)] += 1


def generate_links_json():
    global unique_links, link_count

    links_json: str = 'links: ['
    link_count = 0

    #  Creates list sorted by value in ascending order.
    sorted_list = sorted(unique_links.items(), key=lambda x: x[1])

    #  Creates sorted dictionary.
    sorted_links = dict(sorted_list)

    #  Removes "weak" links (having small values) but keeps at least one existing link between any two nodes.
    for key in list(sorted_links):
        #  Checks lower bound on acceptable number of links.
        if len(sorted_links) <= len(unique_nodes):
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
        source = key[0]
        target = key[1]
        value = sorted_links[key]

        if link_count != 0:
            links_json += ','
        links_json += '{source: "' + str(source) + '", target: "' + str(target) + '", value: ' + str(value) + '}'
        link_count += 1

    links_json += ']'
    return links_json


def generate_nodes_json():
    global unique_nodes

    nodes_json: str = 'nodes: ['
    node_index = 0

    for node in unique_nodes:
        if node_index != 0:
            nodes_json += ','
        value_str = str(unique_nodes[node])
        nodes_json += '{id: "' + str(node) + '", group: ' + value_str + '}'
        node_index += 1

    nodes_json += ']'
    return nodes_json


def finish_parsing(write_to_file):
    links_json = generate_links_json()
    nodes_json = generate_nodes_json()

    node_count = len(unique_nodes)
    print('links', '=>', str(link_count))
    print('nodes', '=>', str(node_count))
    print('articles', '=>', str(article_count))

    topics = 'var topics = {' + nodes_json + ', ' + links_json + ', ' \
        'link_count: "' + str(link_count) + '", ' \
        'node_count: "' + str(node_count) + '", ' \
        'article_count: "' + str(article_count) + '"};'

    textfile = open(write_to_file, "w")
    textfile.write(topics)
    textfile.close()


def extract_graph_data():
    global article_count

    metadata = ijson.parse(open('../data/arxiv-public-datasets.json', 'r'))
    #   Extracts categories
    category_sets = ijson.items(metadata, 'articles.item.categories')

    for category_set in category_sets:
        #  All categories are represented by single string.
        for source in category_set[0].split():
            add_node(source)

            for target in category_set[0].split():
                if source != target:
                    add_link(source, target)

        article_count += 1

    finish_parsing('../data/topics.js')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_graph_data()
