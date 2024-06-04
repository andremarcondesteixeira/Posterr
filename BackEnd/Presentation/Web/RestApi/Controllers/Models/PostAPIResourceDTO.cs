using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Users;
using System;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public sealed record PostAPIResourceDTO : APIResource<PostAPIResourceDTO.EmbeddedObjects>
{
    public bool IsRepost { get; } = false;
    public long Id { get; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public PostAPIResourceDTO(long id, string listPublicationsEndpointUrl)
    {
        Id = id;
        Links.Add("self", [new($"{listPublicationsEndpointUrl}/{Id}")]);
    }

    public static PostAPIResourceDTO FromIPost(IPost post, IUrlHelper Url)
    {
        string listUsersUrl = Url.ActionLink(nameof(UsersController.ListUsers), nameof(UsersController).Replace("Controller", ""))!;
        var author = new UserAPIResourceDTO(post.Author.Id, listUsersUrl)
        {
            Username = post.Author.Username,
        };

        string listPublicationsUrl = Url.ActionLink(nameof(UsersController.ListUsers), nameof(UsersController).Replace("Controller", ""))!;
        return new PostAPIResourceDTO(post.Id, listPublicationsUrl)
        {
            AuthorUsername = author.Username,
            PublicationDate = post.PublicationDate,
            Content = post.Content,
            Embedded = new EmbeddedObjects(author),
        };
    }

    public sealed record EmbeddedObjects(UserAPIResourceDTO Author);
}
