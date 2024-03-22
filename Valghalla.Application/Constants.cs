namespace Valghalla.Application
{
    public static class Constants
    {
        public static class AuditLog
        {
            public const string PrimaryKeyColumn = "Id";
            public const string ParticipantNameColumn = "Name";
            public const string ParticipantBirthdateColumn = "Birthdate";
        }

        public static class Authentication
        {
            public const string Scheme = "ValghallaToken";
            public const string Cookie = "ValghallaTokenKey";
        }

        public static class DefaultCommunicationTemplates
        {
            public const string ConfirmationOfRegistrationStringId = "df1bc563-b6e4-45b5-b6b4-7e76326e926c";
            public const string ConfirmationOfCancellationStringId = "e1bf746f-875e-46c7-9493-ad2610c258a4";
            public const string InvitationStringId = "58cf4b0a-6e55-4045-b255-5613bc966cd7";
            public const string InvitationReminderStringId = "dc92a364-fce3-42b2-b383-0a322b3b18ba";
            public const string TaskReminderStringId = "21b06039-57af-4835-be6e-f227117d91c3";
            public const string RetractedInvitationStringId = "e1f1dd14-bf2a-4ba4-b9e5-d8d98dc9561b";
            public const string RemovedFromTaskStringId = "bb4bb9bf-398a-41b5-a541-8bfc59e1b552";
            public const string RemovedByValidationStringId = "b1a0f0eb-2962-46be-91a2-50753615c1d5";
        }

        public class Validation
        {
            public const int MaximumGeneralStringLength = 255;
            public const int MobileNumberLength = 8;
        }

        public static class WebPages
        {
            public const string WebPageName_FrontPage = "front";
            public const string WebPageName_FAQPage = "faq";
            public const string WebPageName_DisclosureStatementPage = "disclosure-statement";
            public const string WebPageName_DeclarationOfConsentPage = "declaration-of-consent";
            public const string WebPageName_ElectionCommitteeContactInformation = "election-committee-contact-information";
        }

        public static class FileStorage
        {
            public static readonly Guid InternalAuthCertificate = new("c53070b3-075f-465c-96eb-99134d6b670a");
            public static readonly Guid ExternalAuthCertificate = new("baf2fb3e-566d-490e-bbfd-abf16a350219");

            public static readonly Guid MunicipalityLogo = new("978607a3-ac87-4ff5-aa4d-4758c3a84dd0");
            public static readonly Guid InternalCss = new("b65edf68-5326-40e3-916f-aa2b3b341171");
        }
    }
}
