# Posterr

This is a simple Twitter like feed.

Users can create posts and repost posts once. A maximum publication size and a maximum allowed amount of daily publications can be configured.

In this project, I've integrated principles from Clean Architecture and common design patterns associated with Domain Driven Design.

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

## Core Business Rules

Following principles from Clean Architecture, I've implemented the following Enterprise Business Rules:

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

Here are the application business rules:

- Pagination with infinite scroll (first page displays 15 Publications, subsequent pages display 20, but this is configurable)
- Comprehensive display of Post information (user, publication date, content)
- Configurable default user
- Confirmation prompt for reposting actions

# Architecture

The application is divided into a FrontEnd app and a BackEnd app.

The FrontEnd is a NextJS application, and the BackEnd is a modularized monolith written in Asp.Net Core 8.

In the BackEnd side, I used principles from Domain Driven Design, Clean Architecture and Ports and Adapters. The BackEnd application is divided into several layers, aiming to improve the maintainability and scalability of the project:

1. Core: Contains all the business logic
    1. Application/UseCases: Contains the business logic, i.e, the use cases (Application Business Logic in Clean Architecture)
    2. Domain/Entities: Contains the core entities. Those are not database entities, but types that define core behaviors related to the business entities. (Enterprise Business Logic in Clean Architecture)
    3. Shared: Serves as a boundary that allows a clear separation of concerns between different layers without creating tight coupling through direct project dependencies. (Boundaries in Clean Architecture)
2. Infrastructure/Persistence: Contains all the database related logic.
3. Presentation/Web/RestApi: Receives and Responds user requests through HTTP calls
    1. Controllers: Contains the Rest API controllers.
    2. EntryPoint: Starts the application. This is separated from the controllers so that the controllers do not have access to the database repositories, which would break the separation of layers.

# What I would like to do next

1. Write unit tests for the frontend
2. Write e2e tests with Playwright
3. Use Terraform to automatically deploy the application in AWS
4. Setup CI/CD with GitHub Actions
