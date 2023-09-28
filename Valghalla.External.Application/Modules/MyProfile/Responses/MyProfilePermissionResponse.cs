namespace Valghalla.External.Application.Modules.MyProfile.Responses
{
    public sealed record MyProfilePermissionResponse
    {
        public bool TaskLocked { get; init; }
        public bool TaskCompleted { get; init; }
    }
}
