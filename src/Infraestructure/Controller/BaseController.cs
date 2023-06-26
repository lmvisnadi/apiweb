using Infraestructure.Response;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infraestructure.Controller
{

    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult CustomResponse<T>(NewResponse<T> response)
            => response.Status switch
            {
                ResponseStatus.Ok when response.Data is null => Ok(),
                ResponseStatus.Ok => Ok(response.Data),
                ResponseStatus.Created => Created($"{Request.Path.Value}", response.Data),
                ResponseStatus.NoContent => NoContent(),
                ResponseStatus.Forbidden => new ObjectResult(new ApiResponse("Forbidden", Convert.ToInt32(response.Status), response.Messages)) { StatusCode = 403 },
                ResponseStatus.NotFound => NotFound(new ApiResponse("NotFound", Convert.ToInt32(response.Status), response.Messages)),
                ResponseStatus.Unprocessable => UnprocessableEntity(new ApiResponse("BusinessValidation", Convert.ToInt32(response.Status), response.Messages)),
                ResponseStatus.Conflict => Conflict(new ApiResponse("ConflictError", Convert.ToInt32(response.Status), response.Messages)),
                ResponseStatus.Unauthorized => Unauthorized(new ApiResponse("Unauthorized", Convert.ToInt32(response.Status), response.Messages)),
                _ => BadRequest("anErrorOccurred")
            };
    }

}
