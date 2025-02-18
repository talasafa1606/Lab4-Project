Count: http://localhost:5244/api/Library?$apply=aggregate($count%20as%20TotalBooks)
	[{"TotalBooks":5}]

Pagination: http://localhost:5244/api/Library?$skip=2&$top=2
	[{"bookId":3,"title":"Adventures of Huckleberry Finn","authorId":3,"isbn":"9780486280615","publishedYear":1885,"author":null,"loans":[]},{"bookId":4,"title":"test","authorId":4,"isbn":"9780486280615","publishedYear":1997,"author":null,"loans":[]}]

Books By Specific Year:
	ASC: http://localhost:5244/api/Library?$filter=PublishedYear%20eq%201997&$orderby=Title
		[{"bookId":1,"title":"Harry Potter and the Philosopher's Stone","authorId":1,"isbn":"9780747532743","publishedYear":1997,"author":null,"loans":[]},{"bookId":4,"title":"test","authorId":4,"isbn":"9780486280615","publishedYear":1997,"author":null,"loans":[]},{"bookId":5,"title":"test2","authorId":4,"isbn":"9780486280615","publishedYear":1997,"author":null,"loans":[]}]

	DESC: http://localhost:5244/api/Library?$filter=PublishedYear%20eq%201997&$orderby=Title%20desc
		[{"bookId":5,"title":"test2","authorId":4,"isbn":"9780486280615","publishedYear":1997,"author":null,"loans":[]},{"bookId":4,"title":"test","authorId":4,"isbn":"9780486280615","publishedYear":1997,"author":null,"loans":[]},{"bookId":1,"title":"Harry Potter and the Philosopher's Stone","authorId":1,"isbn":"9780747532743","publishedYear":1997,"author":null,"loans":[]}]

Authors By Year:
http://localhost:5244/api/Library/authors?$apply=groupby((BirthYear),%20aggregate($count%20as%20AuthorCount))&$orderby=BirthYear
	[{"BirthYear":1835,"AuthorCount":2},{"BirthYear":1903,"AuthorCount":2},{"BirthYear":1965,"AuthorCount":2}]

Authors By Year and Country:
http://localhost:5244/api/Library/authors?$apply=groupby((BirthYear,%20Country),%20aggregate($count%20as%20AuthorCount))&$orderby=BirthYear,%20Country
	[{"Country":"Lebanon","BirthYear":1835,"AuthorCount":1},{"Country":"United States","BirthYear":1835,"AuthorCount":1},{"Country":"United Kingdom","BirthYear":1903,"AuthorCount":2},{"Country":"United Kingdom","BirthYear":1965,"AuthorCount":2}]








