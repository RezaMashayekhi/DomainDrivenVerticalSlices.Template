namespace DomainDrivenVerticalSlices.Template.Application.Interfaces;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : class
{
}
