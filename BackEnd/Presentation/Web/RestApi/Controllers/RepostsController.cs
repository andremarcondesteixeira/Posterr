using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.CreateNewRepost;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepostsController(CreateNewRepostUseCase createNewRepostUseCase) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewRepostResponseDTO> CreateNewRepost([FromBody] CreateNewRepostRequestDTO request) =>
        createNewRepostUseCase.Run(request);
}
