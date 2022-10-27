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
        self.cited_articles = []
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

    def get_citation_count(self):
        return self.citation_count

    def get_article_id(self):
        return self.article_id


def add_citation(article_id, cited_articles):
    if article_id not in citations:
        # Article is added for the first time.
        citations[article_id] = Citation(article_id, cited_articles)
    else:
        # Article has been added to the dictionary as reference from another article.
        citations[article_id].set_citations(cited_articles)


def get_citations_count():
    return len(citations)