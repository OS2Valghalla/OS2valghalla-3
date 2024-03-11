using Valghalla.Application;
using Valghalla.Application.Enums;

namespace Valghalla.Database.Schema.Data
{
    internal class DefaultCommunicationTemplatesData
    {
        internal static string InitData()
            => $@"
                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.ConfirmationOfRegistrationStringId + @"', 'Standard - bekræftelse på tilmelding', 'Tilmelding som !task_type ved !election',
                '<p>Kære !name</p><p>Tak fordi du vil hjælpe med afviklingen af !election.</p><p>Du er blevet tilmeldt som !task_type.</p><p><strong>Arbejdssted</strong>: !work_location</p><p><strong>Adresse</strong>: !work_location_address</p><p><strong>Mødedato</strong>: !task_date</p><p><strong>Mødetidspunkt</strong>: !task_start</p><p><strong>Opgavebeskrivelse</strong></p><p>!task_type_description</p><p>Hvis du har brug for at ændre dine oplysninger eller håndtere din tilmelding kan det ske på vores valghjemmeside: !external_web.</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Endnu engang tak for din hjælp.</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.ConfirmationOfCancellationStringId + @"', 'Standard - bekræftelse på afmelding', 'Bekræftelse på afmelding ved !election',
                '<p>Kære !name</p><p>Vi bekræfter hermed, at have modtaget din afmelding som !task_type d. !task_date i forbindelse med !election.</p><p>Hvis du gerne vil hjælpe med en anden opgave til valget, kan du logge ind på vores valghjemmeside og se, om der er ledige opgaver: !external_web.</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.InvitationStringId + @"', 'Standard - Invitation til opgave', 'Invitation som !task_type ved !election',
                '<p>Kære !name</p><p>!municipality har udpeget dig som mulig !task_type i forbindelse med !election.</p><p><strong>Arbejdssted</strong>: !work_location</p><p><strong>Adresse</strong>: !work_location_address</p><p><strong>Mødedato</strong>: !task_date</p><p><strong>Mødetidspunkt</strong>: !task_start</p><p>For at sikre os, at du kan komme, bedes du gå ind på nedenstående link og svare på denne invitation.</p><p><strong>Invitation: !invitation</strong></p><p>Vi vil meget gerne have dit svar hurtigt, så vi kan reservere din plads eller tilbyde den til en anden.</p><p><strong>Opgavebeskrivelse</strong></p><p>!task_type_description</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.InvitationReminderStringId + @"', 'Standard - Invitationspåmindelse', 'Påmindelse om tilmelding som !task_type ved !election',
                '<p>Kære !name</p><p>Vi gør opmærksom på, at der nu kun er !days dage til din opgave som !task_type i forbindelse med !election, og at du endnu ikke har meldt tilbage, om du ønsker at hjælpe på:</p><p><strong>Arbejdssted</strong>: !work_location</p><p><strong>Adresse</strong>: !work_location_address</p><p><strong>Mødedato</strong>: !task_date</p><p><strong>Mødetidspunkt</strong>: !task_start</p><p>Vi vil meget gerne have dit svar hurtigt på nedenstående link, så vi kan reservere din plads eller tilbyde den til en anden.</p><p><strong>Invitation</strong>: !invitation</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.TaskReminderStringId + @"', 'Standard - Opgavepåmindelse', 'Påmindelse om deltagelse som !task_type ved !election',
                '<p>Kære !name</p><p>Hermed en påmindelse om at, du har tilmeldt dig som !task_type til !election på</p><p><strong>Arbejdssted</strong>: !work_location</p><p><strong>Adresse</strong>: !work_location_address</p><p><strong>Mødedato</strong>: !task_date</p><p><strong>Mødetidspunkt</strong>: !task_start</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.RetractedInvitationStringId + @"', 'Standard - Annulleret invitation', 'Invitation annulleret som !task_type ved !election',
                '<p>Kære !name</p><p>Din invitation til opgaven som !task_type d. !task_date i forbindelse med !election er blevet annulleret.</p><p>Hvis du har brug for at komme i kontakt med os, kan du finde kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br>Valgsekretariatet<br>!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;
                ";

        internal static string InitData2()
            => $@"
                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.RemovedFromTaskStringId + @"', 'Standard - Fjernet fra opgave', 'Fjernet som !task_type ved !election',
                '<p>Kære !name</p><p>Du er blevet fjernet fra opgaven som !task_type d. !task_date i forbindelse med !election. Det skyldes, at en medarbejder eller teamansvarlig har fjernet dig fra den.</p><p>Hvis du mener, at det er en fejl, kan du kontakte os. Find kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br />Valgsekretariatet<br />!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""CommunicationTemplate""(""Id"", ""Title"", ""Subject"", ""Content"", ""TemplateType"", ""IsDefaultTemplate"", ""CreatedAt"", ""CreatedBy"") 
                values('" + Constants.DefaultCommunicationTemplates.RemovedByValidationStringId + @"', 'Standard - Fjernet pga. nye CPR-oplysninger', 'Fjernet som !task_type ved !election',
                '<p>Kære !name</p><p>Du er blevet fjernet fra opgaven som !task_type d. !task_date i forbindelse med !election. Det skyldes, at vi har modtaget nye oplysninger fra CPR-registeret, som gør, at du ikke længere overholder kravene til denne opgave.</p><p>Hvis du mener, at det er en fejl, kan du kontakte os. Find kontaktoplysninger her: !contact</p><p>Med venlig hilsen<br />Valgsekretariatet<br />!municipality</p>',
                " + (byte)CommunicationTemplateType.DigitalPost + @", true, '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;
                ";
    }
}
