using MediatR;

namespace Valghalla.Application.Abstractions.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : Response
    {
    }
}
