using MediatR;

namespace Valghalla.Application.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : Response
    {
    }
}
