# This script converts downloaded arxiv metadata into graph data suitable for 3D.js.
import ijson


#   Set of tuples (source, target)
unique_links = dict()
unique_nodes = dict()
articles_count = 0

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
    global unique_links

    links_json: str = 'links: ['
    link_index = 0

    for key in unique_links:
        source = key[0]
        target = key[1]
        value = unique_links[key]
        if link_index != 0:
            links_json += ','
        links_json += '{source: "' + str(source) + '", target: "' + str(target) + '", value: ' + str(value) + '}'
        link_index += 1

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

    print('links', '=>', str(len(unique_links)))
    print('nodes', '=>', str(len(unique_nodes)))
    print('articles', '=>', str(articles_count))

    miserables = 'var miserables = {' + nodes_json + ', ' + links_json + '};'

    textfile = open(write_to_file, "w")
    textfile.write(miserables)
    textfile.close()


def extract_graph_data():
    global articles_count

    metadata = ijson.parse(open('arxiv-public-datasets.json', 'r'))
    #   Extracts categories
    category_sets = ijson.items(metadata, 'articles.item.categories')

    for category_set in category_sets:
        #  All categories are represented by single string.
        for source in category_set[0].split():
            add_node(source)

            for target in category_set[0].split():
                if source != target:
                    add_link(source, target)

        articles_count += 1

    finish_parsing('data.js')


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    extract_graph_data()
