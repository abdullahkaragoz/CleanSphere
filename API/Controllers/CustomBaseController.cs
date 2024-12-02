using Microsoft.AspNetCore.Mvc;
using Services;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomBaseController : ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(ServiceResult<T> result)
    {
        if (result.Status == HttpStatusCode.NoContent)
            return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };

        return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
    }

    [NonAction]
    public IActionResult CreateActionResult(ServiceResult result)
    {
        if (result.Status == HttpStatusCode.NoContent)
            return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };

        return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
    }

}
