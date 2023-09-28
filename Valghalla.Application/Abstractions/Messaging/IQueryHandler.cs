using MediatR;

namespace Valghalla.Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Response> where TQuery : IQuery<Response>
    {

    }
}
