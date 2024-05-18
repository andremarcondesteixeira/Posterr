# Posterr - André Teixeira's Full Stack Technical Evaluation

For this evaluation, I've integrated principles from Clean Architecture and common design patterns associated with Domain Driven Design.

I'll start by developing the Core Domain layer (equivalent to the Entities layer in Clean Architecture) using Test-Driven Development (TDD), and then move on to implementing the outer layers.

Throughout the process, I'll be adding comments within the codebase to shed light on my decision-making process.

To kick things off, I began by mapping out the functional requirements and outlining domain entities, setting a solid foundation for development.

## Domain Entities (Back End)

To make the most of the type system, I introduced distinct types to represent various states of the same entity, drawing inspiration from the principles discussed in the book "Domain Modeling Made Functional."

Additionally, I've closely followed Domain Driven Design (DDD) principles to ensure alignment between the code representation and the domain.

For instance, within the domain context, I've made a clear distinction between a Post and a Repost as separate entities.

Here's the breakdown of the core domain entities:

- Post
    - User
    - Publication Date
    - Content
- Repost
    - User
    - Publication Date
    - Original Post
- UnpublishedPost (utilized during new post creation)
    - User
    - Content
- UnpublishedRepost (utilized during new repost creation)
    - User
    - Original Post
- User
    - Username

## Domain Entities (Front End)

On the front-end side, I've decided not to strictly enforce domain rules, delegating this responsibility to the backend.

Validation on the front end will primarily focus on enhancing the user experience, without imposing strict enforcement by the type system. Validation will only occur during form submissions for creating new posts.

This approach will expedite the development of the front end.

## Core Business Rules

Following principles from Clean Architecture, I've identified the following Enterprise Business Rules:

- Usernames must be unique and alphanumeric (ensured at the database level)
- Posts are restricted to text content
- Publications cannot be deleted (applies to both Posts and Reposts)
- Posting restrictions include:
    - A user may not exceed five publications (Posts or Reposts) per day
    - Post content is limited to 777 characters
- Reposting restrictions include:
    - Posts may only be reposted once by a user
    - Users cannot repost their own posts
    - Reposts cannot be reposted
- Publications (Posts or Reposts) should be paginated together, sorted as follows:
    - When sorted by "latest," older publications appear first
    - When sorted by "trending," the most reposted Posts are prioritized
- Search functionality for Posts by content, with the following specifications:
    - Exact text search string match
    - Reposts are excluded from search results

# Application Business Rules

The pagination guideline, stipulating 15 Publications on the first page and 20 on subsequent pages, could be considered an Application Business Rule, albeit subject to interpretation. For this assessment's purposes, I'll treat it as such.

Here are the application business rules:

- Pagination with infinite scroll (first page displays 15 Publications, subsequent pages display 20)
- Comprehensive display of Post information (user, publication date, content)
- No authentication mechanism
- Absence of users CRUD operations
- Configurable default user
- Confirmation prompt for reposting actions
