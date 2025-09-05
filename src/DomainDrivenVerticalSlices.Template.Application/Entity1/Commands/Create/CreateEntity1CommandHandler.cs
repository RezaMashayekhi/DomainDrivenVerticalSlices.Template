namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Mappings;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.Events;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateEntity1CommandHandler(
    IEntity1Repository entity1Repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateEntity1CommandHandler> logger,
    IPublisher publisher) : IRequestHandler<CreateEntity1Command, Result<Entity1Dto>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<CreateEntity1CommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task<Result<Entity1Dto>> Handle(CreateEntity1Command request, CancellationToken cancellationToken)
    {
        var valueObject1Result = ValueObject1.Create(request.Property1);
        if (!valueObject1Result.IsSuccess)
        {
            _logger.LogError("An error occurred: {ErrorMessage}", valueObject1Result.CheckedError.ErrorMessage);
            return Result<Entity1Dto>.Failure(valueObject1Result.CheckedError);
        }

        var entity1Result = Entity1.Create(valueObject1Result.Value);
        if (!entity1Result.IsSuccess)
        {
            _logger.LogError("An error occurred: {ErrorMessage}", entity1Result.CheckedError.ErrorMessage);
            return Result<Entity1Dto>.Failure(entity1Result.CheckedError);
        }

        try
        {
            var entity1 = await _entity1Repository.AddAsync(entity1Result.Value, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new Entity1CreatedEvent(entity1.Id), cancellationToken);

            _logger.LogInformation("Entity1 created with id {Entity1Id}.", entity1.Id);
            var entity1Dto = entity1.MapToDto();
            return Result<Entity1Dto>.Success(entity1Dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating Entity1");
            return Result<Entity1Dto>.Failure(Error.Create(ErrorType.OperationFailed, "Error creating Entity1"));
        }
    }
}
