namespace Valghalla.Application.Abstractions.Messaging
{
    public record ConfirmationCommand<TResponse> : ConfirmationCommand, ICommand<TResponse> where TResponse : Response
    {
    }

    public record ConfirmationCommand
    {
        public bool Confirmed { get; set; }
    }
}
