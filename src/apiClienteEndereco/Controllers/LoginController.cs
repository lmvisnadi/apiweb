using apiClienteEndereco.Payload.Login;
using Infraestructure.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiClienteEndereco.Controllers
{
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        public LoginController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Guid>> Login([FromBody] LoginPostPayload payload)
            => CustomResponse(await _mediator.Send(payload.AsCommand()));

    }
}
