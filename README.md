# Posterr Backend - André Teixeira's Technical Evalulation

For this test, I decided to borrow some concepts from Clean Architecture and some design patterns commonly used when applying Domain Driven Design.

I'll start developing the Core Domain layer (Entities layer in Clean Architecture) using TDD and then proceed to the outermost layers.

I'll try to use comments in the code to explain my thought process.

First thing I did was to map the functional requisites and the domain entities, so that I had a plan to follow.

## The Domain Entities

I decided to use the type system in my favor, so I created different types for different states of the same entity (the book "Domain Modeling Made Functional" is a good reference on this topic).

Also, I borrow from DDD the concept to map the domain as closely as possible in the code.

For example: The way I see it, a Post and a Repost are different things from the domain point of view.

This is the list of the base domain entities I identified:

- Post
	- User
	- Publication Date
	- Content
- Repost
	- User
	- Publication Date
	- Original Post
- UnpublishedPost (used when creating new posts)
	- User
	- Content
- UnpublishedRepost (used when creating new reposts)
	- User
	- Original Post
- User
	- Username 

## The Core Business Rules

A.K.A Enterprise Business Rules in Clean Architecture.

Apart from the basic CRUD operations, those are the business rules I identified, in the way I interpreted them, sorted and grouped in a way that makes sense for me:

- Usernames are unique (alphanumeric) (This will be guaranteed only at database level, because this test asks not to make a Users CRUD)
- Posts are text only
- Publications cannot be deleted (or only Posts? I will consider both)
- Restrictions when posting:
	- A User cannot make more than 5 Publications a day (Posts or Reposts)
	- Post content size is limited to 777 characters
- Restrictions when reposting:
	- Posts can be reposted only once by User
	- User cannot repost their own Posts
	- Reposts cannot be reposted
- Publications (Posts or Reposts) should be paginated together
	- When sorting by "latest", list older Publications first
	- When sorting by "trending", list most reposted Posts first (If I get it, "trending" filters Reposts out? I guess so)
- Search Posts by content
	- Use exact text search string match
	- Reposts are filtered out

# The Application Business Rules

It is debatable whether the rule about showing 15 Publications in the first page and 20 in the subsequent pages is an Application Business Rule, because the "Business" here is the Application itself.

But I prefer to not think too much about this and proceed with the test considering it as an Application Business Rule.

- Pagination with infinite scroll (first page shows 15 Publications, other pages show 20)
- Show complete Post information (user, publication date, content)
- No auth
- No users CRUD
- Configurable default user
- Confirm repost intention
