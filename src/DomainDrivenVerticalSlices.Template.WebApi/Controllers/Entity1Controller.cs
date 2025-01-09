namespace DomainDrivenVerticalSlices.Template.WebApi.Controllers;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class Entity1Controller(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Entity1Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetEntity1ByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleErrors(result);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetEntity1AllQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleErrors(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(IEnumerable<Entity1Dto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(string property1, CancellationToken cancellationToken = default)
    {
        var query = new ListEntity1ByProperty1Query(property1);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleErrors(result);
    }

    [HttpGet("find")]
    [ProducesResponseType(typeof(Entity1Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FindByProperty1(string property1, CancellationToken cancellationToken = default)
    {
        var query = new FindEntity1ByProperty1Query(property1);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleErrors(result);
    }

    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(Entity1Dto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateEntity1Command command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess switch
        {
            true => CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value),
            false => HandleErrors(result),
        };
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateEntity1Command command, CancellationToken cancellationToken = default)
    {
        if (id != command.Entity1.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleErrors(result);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteEntity1Command(id);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : HandleErrors(result);
    }

    private IActionResult HandleErrors(IResult result)
    {
        return result.CheckedError.ErrorType switch
        {
            ErrorType.NotFound => NotFound(result.Error),
            ErrorType.InvalidInput => BadRequest(result.Error),
            ErrorType.OperationFailed => BadRequest(result.Error),
            _ => BadRequest(Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.")),
        };
    }
}
