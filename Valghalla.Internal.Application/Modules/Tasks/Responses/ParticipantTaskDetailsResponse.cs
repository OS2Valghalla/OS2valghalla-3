﻿namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record ParticipantTaskDetailsResponse
    {
        public string? ParticipantName { get; set; }
        public string? ParticipantCpr { get; set; }
        public int? ParticipantAge { get; set; }
        public string? ParticipantPhoneNumber { get; set; }
        public string? ParticipantEmail { get; set; }
        public string? ParticipantAddress { get; set; }
        public bool ParticipantProtectedAddress { get; set; }
        public string? ParticipantSpecialDiets { get; set; }
        public bool ParticipantDigitalPostStatus { get; set; }
        public DateTime? ParticipantBirthDate { get; set; }
        public string? ParticipantUserName { get; set; }
        public string? AreaName { get; set; }
        public string? TeamName { get; set; }
        public string? WorkLocation { get; set; }
        public string? VotingArea { get; set; }
        public string? TaskTypeName { get; set; }
        public DateTime? TaskDate { get; set; }
        public string? TaskStatus { get; set; }        
        public TimeSpan? TaskStartTime { get; set; }
        public string? TaskPayment { get; set; }

    }
}
