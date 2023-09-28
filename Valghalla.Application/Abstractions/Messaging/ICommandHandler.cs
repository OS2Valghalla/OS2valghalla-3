using MediatR;
namespace Valghalla.Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TResponse: Response
        where TCommand : ICommand<TResponse>
    {

    }
}
