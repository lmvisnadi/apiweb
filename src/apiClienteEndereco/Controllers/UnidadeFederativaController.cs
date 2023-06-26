using apiClienteEndereco.Payload.UnidadeFederativa;
using Application.CommandSide.Commands.RemoveUnidadeFederativa;
using Application.QuerySide.GetUnidadeFederativa;
using Application.QuerySide.ListUnidadeFederativa;
using Infraestructure.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiClienteEndereco.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UnidadeFederativaController : BaseController
    {
        public UnidadeFederativaController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Create([FromBody] UnidadeFederativaCreatePayload payLoad)
          => CustomResponse(await _mediator.Send(payLoad.AsCommand()));

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GetUnidadeFederativaQuery), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetUnidadeFederativaQuery>> Get([FromRoute] Guid id)
        => CustomResponse(await _mediator.Send(new GetUnidadeFederativaQuery(id)));

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Update(
            [FromRoute] Guid id,
            [FromBody] UnidadeFederativaUpdatePayload payload
            )
            => CustomResponse(await _mediator.Send(payload.AsCommand(id)));

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Guid id)
            => CustomResponse(await _mediator.Send(new RemoveUnidadeFederativaCommand(id)));

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ListUnidadeFederativaViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListUnidadeFederativaViewModel>>> List([FromQuery] UnidadeFederativaListPayload payload)
            => CustomResponse(await _mediator.Send(payload.AsQuery()));
    }
}
