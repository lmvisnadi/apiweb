using apiClienteEndereco.Payload.Municipio;
using Application.CommandSide.Commands.RemoveMuncipio;
using Application.QuerySide.GetMunicipio;
using Application.QuerySide.ListMunicipio;
using Infraestructure.Controller;
using Infraestructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiClienteEndereco.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class MunicipioController : BaseController
    {
        public MunicipioController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Create([FromBody] MunicipioCreatePayload payLoad)
            => CustomResponse(await _mediator.Send(payLoad.AsCommand()));

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GetMunicipioQuery), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetMunicipioQuery>> Get([FromRoute] Guid id)
            => CustomResponse(await _mediator.Send(new GetMunicipioQuery(id)));

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GetMunicipioQuery), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Update(
            [FromRoute] Guid id,
            [FromBody] MunicipioUpdatePayload payload
        )
            => CustomResponse(await _mediator.Send(payload.AsCommand(id)));

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Guid id)
            => CustomResponse(await _mediator.Send(new RemoveMunicipioCommand(id)));

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ListMunicipioViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListMunicipioViewModel>>> List(
            [FromQuery] PaginationOptions? paginationOptions, [FromQuery] MunicipioListPayload payload)
            => CustomResponse(await _mediator.Send(payload.AsQuery(paginationOptions)));
    }
}
