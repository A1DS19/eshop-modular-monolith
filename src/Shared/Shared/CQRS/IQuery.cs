namespace Shared.CQRS;

public interface IQuery<out T> : IRequest<T>, ICommonOperationRequest<T>
    where T : notnull { }
