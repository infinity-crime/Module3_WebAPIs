using BooksKeeper.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BooksKeeper.Application.Common.Error;

namespace BooksKeeper.Authors.WebAPI.Controllers.Common
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result);

            return Problem(result.Error!);
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();

            return Problem(result.Error!);
        }

        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.AccessUnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(
                statusCode: statusCode,
                title: error.Description,
                detail: error.Code
                );
        }
    }
}
