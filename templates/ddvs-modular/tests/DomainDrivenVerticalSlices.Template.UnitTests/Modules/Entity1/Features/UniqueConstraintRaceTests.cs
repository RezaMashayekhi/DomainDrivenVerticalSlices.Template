namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1.Features;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public sealed class UniqueConstraintRaceTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CompetingWrite_AfterPrecheck_ReturnsConflict(bool update)
    {
        CompetingWrite interceptor = new();
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync(interceptor);
        CreateEntity1Handler create = new(scope.DbContext);
        Result<CreateEntity1Response> initial = await create.HandleAsync(new("initial"), CancellationToken.None);
        interceptor.Enabled = true;

        Result outcome = update
            ? await new UpdateEntity1Handler(scope.DbContext).HandleAsync(initial.Value.Id, new("raced"), CancellationToken.None)
            : await create.HandleAsync(new("raced"), CancellationToken.None);

        Assert.True(outcome.IsFailure);
        Assert.Equal(ErrorType.Conflict, outcome.Error.Type);
    }

    private sealed class CompetingWrite : SaveChangesInterceptor
    {
        public bool Enabled { get; set; }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (Enabled)
            {
                Enabled = false;
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(eventData.Context!.Database.GetConnectionString()).Options;
                await using AppDbContext competitor = new(options);
                competitor.Entities1.Add(Entity1.Create(ValueObject1.Create("raced").Value).Value);
                await competitor.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
