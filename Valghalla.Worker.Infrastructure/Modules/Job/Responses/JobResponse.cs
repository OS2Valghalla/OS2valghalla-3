namespace Valghalla.Worker.Infrastructure.Modules.Job.Responses
{
    public sealed record JobResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public Guid? JobId { get; init; }
    }
}
