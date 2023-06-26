using apiClienteEndereco.Payload.Cliente;
using Application.CommandSide.Commands.RemoveCliente;
using Application.QuerySide.GetCliente;
using Application.QuerySide.ListCliente;
using Infraestructure.Controller;
using Infraestructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiClienteEndereco.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ClienteController : BaseController
    {
        public ClienteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Guid>> Create([FromBody] ClienteCreatePayload payload)
        => CustomResponse(await _mediator.Send(payload.AsCommand()));

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GetClienteViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetClienteViewModel>> Get([FromRoute] Guid id)
        => CustomResponse(await _mediator.Send(new GetClienteQuery(id)));

        [HttpGet]
        [ProducesResponseType(typeof(Page<ListClienteViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListClienteViewModel>> List(
            [FromQuery] PaginationOptions? paginationOptions,
            [FromQuery] ClienteListPayload payload
            )
            => CustomResponse(await _mediator.Send(payload.AsQuery(paginationOptions)));

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetClienteQuery>> Delete([FromRoute] Guid id)
            => CustomResponse(await _mediator.Send(new RemoveClienteCommand(id)));

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] ClienteUpdatePayload payload
        )
        => CustomResponse(await _mediator.Send(payload.AsCommand(id)));
    }
}
