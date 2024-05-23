﻿using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.PaginatePublications;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase,
    PaginatePublicationsUseCase paginatePublicationsUseCase
) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewPostResponseDTO> CreateNewPost([FromBody] CreateNewPostRequestDTO request) =>
        createNewPostUseCase.Run(request);

    [HttpPost]
    public Task<CreateNewRepostResponseDTO> CreateNewRepost([FromBody] CreateNewRepostRequestDTO request) =>
        createNewRepostUseCase.Run(request);

    [HttpGet]
    public Task<IList<PaginatePublicationsResponseItemDTO>> PaginatePosts([FromBody] PaginatePublicationsRequestDTO request) =>
        paginatePublicationsUseCase.Run(request);
}
