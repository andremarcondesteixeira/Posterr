using Microsoft.AspNetCore.Mvc;

namespace Posterr.Presentation.Web.RestApi.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet(Name = nameof(ListUsers))]
    public IActionResult ListUsers()
    {
        return Ok("");
    }

    [HttpGet("{userId}")]
    public IActionResult GetUserById([FromRoute] long userId)
    {
        return Ok("");
    }
}
