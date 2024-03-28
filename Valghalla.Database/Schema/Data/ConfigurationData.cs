using Valghalla.Application.Configuration;

namespace Valghalla.Database.Schema.Data
{
    internal class ConfigurationData
    {
        internal static string InitConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.Cvr)}', '')
                on conflict do nothing;
                ";

        internal static string InitKomkodConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.Komkod)}', '')
                on conflict do nothing;
                ";

        internal static string InitSmsSenderConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.SmsSender)}', '')
                on conflict do nothing;
                ";

        internal static string InitTaskReminderDayConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.TaskReminderDay)}', '5')
                on conflict do nothing;
                ";

        internal static string InitMailSenderConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.MailSender)}', '')
                on conflict do nothing;
                ";

        internal static string InitMailAddressConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.MailAddress)}', '')
                on conflict do nothing;
                ";

        internal static string InitDigitalPostConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.DigitalPostCvr)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.DigitalPostSender)}', '')
                on conflict do nothing;
                ";

        internal static string InitAuthConfigrationData()
           => $@"
                insert into ""Configuration"" values('{nameof(InternalAuthConfiguration)}.{nameof(InternalAuthConfiguration.Issuer)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(InternalAuthConfiguration)}.{nameof(InternalAuthConfiguration.Authority)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(InternalAuthConfiguration)}.{nameof(InternalAuthConfiguration.SigningCertificatePassword)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(ExternalAuthConfiguration)}.{nameof(ExternalAuthConfiguration.Issuer)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(ExternalAuthConfiguration)}.{nameof(ExternalAuthConfiguration.Authority)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(ExternalAuthConfiguration)}.{nameof(ExternalAuthConfiguration.SigningCertificatePassword)}', '')
                on conflict do nothing;
                ";

        internal static string AddJobRoleDescriptionConfiguraitonData()
            => $@"
                 insert into ""Configuration"" values('{nameof(InternalAuthConfiguration)}.{nameof(InternalAuthConfiguration.JobRoleDescription)}', '')
                 on conflict do nothing;
                 ";

        internal static string InitReplyToMailConfigrationData()
            => $@"
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.ReplyToMailSender)}', '')
                on conflict do nothing;
                insert into ""Configuration"" values('{nameof(AppConfiguration)}.{nameof(AppConfiguration.ReplyToMailAddress)}', '')
                on conflict do nothing;
                ";
    }
}
