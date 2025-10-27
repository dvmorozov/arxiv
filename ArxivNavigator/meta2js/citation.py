########################################################################################################################
# File "citation.py"
# Copyright © Dmitry Morozov 2022
# If you want to use this file please contact me by dvmorozov@hotmail.com.
########################################################################################################################


citations = dict()


class Citation(object):
    def __init__(self, article_id, cited_articles):
        self.article_id = article_id
        self.citation_count = 0
        self.cited_articles = []                # Set of article ids. which are referenced by given article.
        self.cited_by_articles = set()          # Set of article ids. which references this article.
        self.set_citations(cited_articles)

    def add_citation_count(self, citation_count):
        self.citation_count += citation_count

    def set_citations(self, cited_articles):
        # Must be set only once.
        assert(len(self.cited_articles) == 0)

        self.cited_articles = cited_articles

        for cited_article in cited_articles:
            if cited_article not in citations:
                citations[cited_article] = Citation(cited_article, [])

            citations[cited_article].add_citation_count(self.citation_count + 1)
            citations[cited_article].add_citing_article(self.article_id)

    def add_citing_article(self, article_id):
        self.cited_by_articles.add(article_id)

    def get_citation_count(self):
        return self.citation_count

    def get_cited_by_articles(self):
        return self.cited_by_articles

    def get_article_id(self):
        return self.article_id

    def get_json(self) -> str:
        return '{article_id: "' + str(self.article_id) + '", ' \
                'citation_count: "' + str(self.citation_count) + '"}'


def add_citation(article_id, cited_articles):
    if article_id not in citations:
        # Article is added for the first time.
        citations[article_id] = Citation(article_id, cited_articles)
    else:
        # Article has been added to the dictionary as reference from another article.
        citations[article_id].set_citations(cited_articles)


def get_citations_count():
    return len(citations)


def generate_citations_json() -> str:
    global citations

    citations_json: str = 'articles: ['
    citations_count = 0

    for key in citations:
        if citations_count != 0:
            citations_json += ', '

        citation = citations[key]

        citations_json += citation.get_json()
        citations_count += 1

    citations_json += ']'
    return citations_json
