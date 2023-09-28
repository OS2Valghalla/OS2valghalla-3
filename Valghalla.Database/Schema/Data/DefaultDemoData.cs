using Valghalla.Application.Enums;
using Valghalla.Application.TaskValidation;

namespace Valghalla.Database.Schema.Data
{
    internal class DefaultDemoData
    {
        internal static string InitData()
           => $@"
                insert into ""SpecialDiet""(""Id"", ""Title"", ""CreatedAt"", ""CreatedBy"")
                values('c14739ba-244d-43e4-a486-3f87d81fba27', 'Glutenfri kost', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;
                
                insert into ""SpecialDiet""(""Id"", ""Title"", ""CreatedAt"", ""CreatedBy"")
                values('994e296c-9efb-4300-9894-99e58128714d', 'Vegetarisk kost', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Area""(""Id"", ""Name"", ""CreatedAt"", ""CreatedBy"")
                values('a8124956-ab19-47d0-8a66-cf582b13f999', 'Nord', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Area""(""Id"", ""Name"", ""CreatedAt"", ""CreatedBy"")
                values('bfcb4577-51df-4b52-9530-f8d1db9f6742', 'Syd', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Area""(""Id"", ""Name"", ""CreatedAt"", ""CreatedBy"")
                values('f6158df8-e1d2-4ca4-ba8c-1c550d210bdb', 'Vest', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Area""(""Id"", ""Name"", ""CreatedAt"", ""CreatedBy"")
                values('c588a452-9eb9-44a6-b231-c84ff70e26e6', 'Øst', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('472c3c0b-5379-4ab1-9ac2-6c9cfaac6a33', 'Alternativet', 'Å', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('51655671-18a9-47d9-ba7c-adb071105e77', 'Borgerservice', 'BS', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('86a5c9f5-413d-403b-88fb-9a7ecdd3a9c9', 'Danmarksdemokraterne', 'Æ', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('9e6dfce4-2c33-4b06-a0f2-8f1d2cec0297', 'Dansk Folkeparti', 'O', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('ef99ac18-1461-448e-b24c-6388fe411251', 'Det Konservative Folkeparti', 'C', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('c78a424a-97a1-4262-8c35-e39f2b473ca2', 'Enhedslisten', 'Ø', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('eff9c38c-cb69-4c79-9caa-195ef109e07a', 'Frivillige', 'FRI', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('b4bad740-829e-467d-94fb-f2295cce38cf', 'IT-afdelingen', 'IT', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('4bc02d83-9ab8-4447-a379-5d1412583083', 'Liberal Alliance', 'I', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('4b3484f5-54ca-4e39-af2d-cbf3e8680e43', 'Medarbejdere', 'MED', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('aa79e663-190a-44e0-8e36-460075fa74f5', 'Moderaterne', 'M', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('3bcd8ab1-9c31-4a99-b020-608d354df40c', 'Nye Borgerlige', 'D', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('02e079ad-43bd-4295-aabd-8d1f431e6d29', 'Radikale Venstre', 'BS', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('d93e6411-7131-4d15-aeb0-58d82fd0603a', 'Socialdemokratiet', 'F', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('aab4954d-1dc5-4360-bdab-d13b01c918d4', 'Socialistisk Folkeparti', 'A', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""Team""(""Id"", ""Name"", ""ShortName"", ""CreatedAt"", ""CreatedBy"")
                values('a8f8f048-92f0-4600-8fa8-0c226784980d', 'Venstre', 'V', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('ec0aea3e-a557-438d-8bc8-c7b4e96d395f', 'Brevstemmemodtager', 'BREV', '08:00:00', '16:00:00', 800, false, false, true, '<p>Din opgave er at hjælpe borgeren med brevafstemning på et plejehjem / lokalcenter eller bosted.</p><p></p><p>Du skal besøge borgere, som er syge – enten fysisk eller psykisk – og derfor ikke kan komme til valgstedet. Det er derfor vigtigt, at du har tålmodighed og forståelse for, at nogle borgere har brug for lidt ekstra tid eller hjælp til at afgive deres stemme.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('bce78adb-6531-45fb-89e7-2a58d89198b8', 'Fintæller', 'FINT', '17:00:00', '23:59:00', 800, true, false, true, '<p>Fintællere har til opgave at sikre, at stemmeoptællingen er korrekt.</p><p></p><p>I løbet af dagen vil du blandt andet stå for påtælling, vejning af stemmesedler og tjek af tidligere optællinger.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('fc43acca-6c53-49a1-9773-428b81cd4cc2', 'Tilforordnet', 'TILF', '07:00:00', '23:59:00', 800, false, false, true, '<p>Frivillige tilforordnede hjælper med at tage imod vælgerne, krydse dem af i listerne og udlevere stemmesedler.</p><p></p><p>Som frivillig er du også med til at hjælpe vælgerne med at afgive deres stemme og sikre, at stemmesedlerne kommer i valgurnerne. Samtidig er de mange frivillige med til at sikre, at det hele forløber efter reglerne, og så er de med til at skabe den gode stemning på valgstederne.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('abece9d6-6d2e-4770-b23f-e2e9c2f1770d', 'Valgsekretær', 'VSEK', '07:00:00', '23:59:00', 800, true, true, true, '<p>En valgsekretær har det praktiske ansvar for afvikling af valget på valgstedet.</p><p></p><p>Sammen med valgstyrerformanden skal du fordele opgaverne mellem valgstyrere og tilforordnede og generelt varetage den praktiske afvikling af valget på valgstedet.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('e1b8583e-7585-4063-ac72-51854170ca16', 'Valgstyrer', 'VSTY', '07:00:00', '23:59:00', 800, false, true, true, '<p>En valgstyrer er en politisk udpeget vælger, der bistår i afviklingen af valget.</p><p></p><p>I løbet af dagen skal du hjælpe med de opgaver, som aftales med valgstyrerformanden. Du skal desuden overvære optællingen af valgresultatet og kan også deltage i den.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""TaskType""(""Id"", ""Title"", ""ShortName"", ""StartTime"", ""EndTime"", ""Payment"", ""ValidationNotRequired"", ""Trusted"", ""SendingReminderEnabled"", ""Description"", ""CreatedAt"", ""CreatedBy"")
                values('aa3ce7ef-ae92-4036-b968-64838328cb44', 'Valgstyrerformand', 'VSTYF', '07:00:00', '23:59:00', 800, false, true, true, '<p>Valgstyrerformanden er en politisk udpeget vælger, der har det samlede ansvar for valgets afvikling på valgstedet sammen med de øvrige valgstyrere og i samarbejde med valgsekretæren.</p>', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""WorkLocation""(""Id"", ""AreaId"", ""Title"", ""Address"", ""PostalCode"", ""City"", ""CreatedAt"", ""CreatedBy"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'a8124956-ab19-47d0-8a66-cf582b13f999', 'Forsamlingshuset', 'Nørre Allé 12', '4272', 'Korsbæk', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""WorkLocation""(""Id"", ""AreaId"", ""Title"", ""Address"", ""PostalCode"", ""City"", ""CreatedAt"", ""CreatedBy"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'bfcb4577-51df-4b52-9530-f8d1db9f6742', 'Idrætshallen', 'Sportsvej 7', '4272', 'Korsbæk', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""WorkLocation""(""Id"", ""AreaId"", ""Title"", ""Address"", ""PostalCode"", ""City"", ""CreatedAt"", ""CreatedBy"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'f6158df8-e1d2-4ca4-ba8c-1c550d210bdb', 'Rådhuset', 'Rådhuspladsen 4', '4272', 'Korsbæk', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""WorkLocation""(""Id"", ""AreaId"", ""Title"", ""Address"", ""PostalCode"", ""City"", ""CreatedAt"", ""CreatedBy"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'c588a452-9eb9-44a6-b231-c84ff70e26e6', 'Skolen', 'Skolegade 4', '4272', 'Korsbæk', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'ec0aea3e-a557-438d-8bc8-c7b4e96d395f')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'fc43acca-6c53-49a1-9773-428b81cd4cc2')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'abece9d6-6d2e-4770-b23f-e2e9c2f1770d')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'e1b8583e-7585-4063-ac72-51854170ca16')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'aa3ce7ef-ae92-4036-b968-64838328cb44')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'ec0aea3e-a557-438d-8bc8-c7b4e96d395f')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'fc43acca-6c53-49a1-9773-428b81cd4cc2')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'abece9d6-6d2e-4770-b23f-e2e9c2f1770d')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'e1b8583e-7585-4063-ac72-51854170ca16')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'aa3ce7ef-ae92-4036-b968-64838328cb44')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'ec0aea3e-a557-438d-8bc8-c7b4e96d395f')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'fc43acca-6c53-49a1-9773-428b81cd4cc2')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'abece9d6-6d2e-4770-b23f-e2e9c2f1770d')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'e1b8583e-7585-4063-ac72-51854170ca16')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'aa3ce7ef-ae92-4036-b968-64838328cb44')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'ec0aea3e-a557-438d-8bc8-c7b4e96d395f')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'fc43acca-6c53-49a1-9773-428b81cd4cc2')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'abece9d6-6d2e-4770-b23f-e2e9c2f1770d')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'e1b8583e-7585-4063-ac72-51854170ca16')
                on conflict do nothing;

                insert into ""WorkLocationTaskTypes""(""WorkLocationId"", ""TaskTypeId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'aa3ce7ef-ae92-4036-b968-64838328cb44')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '472c3c0b-5379-4ab1-9ac2-6c9cfaac6a33')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '51655671-18a9-47d9-ba7c-adb071105e77')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '86a5c9f5-413d-403b-88fb-9a7ecdd3a9c9')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '9e6dfce4-2c33-4b06-a0f2-8f1d2cec0297')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'ef99ac18-1461-448e-b24c-6388fe411251')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'c78a424a-97a1-4262-8c35-e39f2b473ca2')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'eff9c38c-cb69-4c79-9caa-195ef109e07a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'b4bad740-829e-467d-94fb-f2295cce38cf')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '4bc02d83-9ab8-4447-a379-5d1412583083')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '4b3484f5-54ca-4e39-af2d-cbf3e8680e43')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'aa79e663-190a-44e0-8e36-460075fa74f5')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '3bcd8ab1-9c31-4a99-b020-608d354df40c')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', '02e079ad-43bd-4295-aabd-8d1f431e6d29')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'd93e6411-7131-4d15-aeb0-58d82fd0603a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'aab4954d-1dc5-4360-bdab-d13b01c918d4')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('3723ddcb-8c64-44dc-9125-bc560e9b4ec2', 'a8f8f048-92f0-4600-8fa8-0c226784980d')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '472c3c0b-5379-4ab1-9ac2-6c9cfaac6a33')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '51655671-18a9-47d9-ba7c-adb071105e77')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '86a5c9f5-413d-403b-88fb-9a7ecdd3a9c9')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '9e6dfce4-2c33-4b06-a0f2-8f1d2cec0297')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'ef99ac18-1461-448e-b24c-6388fe411251')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'c78a424a-97a1-4262-8c35-e39f2b473ca2')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'eff9c38c-cb69-4c79-9caa-195ef109e07a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'b4bad740-829e-467d-94fb-f2295cce38cf')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '4bc02d83-9ab8-4447-a379-5d1412583083')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '4b3484f5-54ca-4e39-af2d-cbf3e8680e43')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'aa79e663-190a-44e0-8e36-460075fa74f5')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '3bcd8ab1-9c31-4a99-b020-608d354df40c')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', '02e079ad-43bd-4295-aabd-8d1f431e6d29')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'd93e6411-7131-4d15-aeb0-58d82fd0603a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'aab4954d-1dc5-4360-bdab-d13b01c918d4')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('08509e77-8ad8-43ac-82f9-f4f15a23cb84', 'a8f8f048-92f0-4600-8fa8-0c226784980d')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '472c3c0b-5379-4ab1-9ac2-6c9cfaac6a33')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '51655671-18a9-47d9-ba7c-adb071105e77')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '86a5c9f5-413d-403b-88fb-9a7ecdd3a9c9')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '9e6dfce4-2c33-4b06-a0f2-8f1d2cec0297')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'ef99ac18-1461-448e-b24c-6388fe411251')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'c78a424a-97a1-4262-8c35-e39f2b473ca2')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'eff9c38c-cb69-4c79-9caa-195ef109e07a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'b4bad740-829e-467d-94fb-f2295cce38cf')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '4bc02d83-9ab8-4447-a379-5d1412583083')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '4b3484f5-54ca-4e39-af2d-cbf3e8680e43')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'aa79e663-190a-44e0-8e36-460075fa74f5')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '3bcd8ab1-9c31-4a99-b020-608d354df40c')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', '02e079ad-43bd-4295-aabd-8d1f431e6d29')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'd93e6411-7131-4d15-aeb0-58d82fd0603a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'aab4954d-1dc5-4360-bdab-d13b01c918d4')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('6247997e-433d-4ca7-bf60-a146aaf50a7d', 'a8f8f048-92f0-4600-8fa8-0c226784980d')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '472c3c0b-5379-4ab1-9ac2-6c9cfaac6a33')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '51655671-18a9-47d9-ba7c-adb071105e77')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '86a5c9f5-413d-403b-88fb-9a7ecdd3a9c9')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '9e6dfce4-2c33-4b06-a0f2-8f1d2cec0297')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'ef99ac18-1461-448e-b24c-6388fe411251')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'c78a424a-97a1-4262-8c35-e39f2b473ca2')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'eff9c38c-cb69-4c79-9caa-195ef109e07a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'b4bad740-829e-467d-94fb-f2295cce38cf')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '4bc02d83-9ab8-4447-a379-5d1412583083')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '4b3484f5-54ca-4e39-af2d-cbf3e8680e43')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'aa79e663-190a-44e0-8e36-460075fa74f5')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '3bcd8ab1-9c31-4a99-b020-608d354df40c')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', '02e079ad-43bd-4295-aabd-8d1f431e6d29')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'd93e6411-7131-4d15-aeb0-58d82fd0603a')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'aab4954d-1dc5-4360-bdab-d13b01c918d4')
                on conflict do nothing;

                insert into ""WorkLocationTeams""(""WorkLocationId"", ""TeamId"")
                values('75a7d9df-0c04-42a3-87f6-9863429a4d63', 'a8f8f048-92f0-4600-8fa8-0c226784980d')
                on conflict do nothing;

                insert into ""ElectionType""(""Id"", ""Title"", ""CreatedAt"", ""CreatedBy"")
                values('cd2e73a2-07fc-496c-8cc2-965ab968acca', 'EU-parlamentsvalg', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('cd2e73a2-07fc-496c-8cc2-965ab968acca', '" + TaskValidationRule.Age18.Id + @"')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('cd2e73a2-07fc-496c-8cc2-965ab968acca', '" + TaskValidationRule.ResidencyMunicipality.Id + @"')
                on conflict do nothing;

                insert into ""ElectionType""(""Id"", ""Title"", ""CreatedAt"", ""CreatedBy"")
                values('af6609c4-17dd-4c66-8ae9-423ff75efd83', 'Folketingsvalg og folkeafstemning', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('af6609c4-17dd-4c66-8ae9-423ff75efd83', '" + TaskValidationRule.Age18.Id + @"')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('af6609c4-17dd-4c66-8ae9-423ff75efd83', '" + TaskValidationRule.ResidencyMunicipality.Id + @"')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('af6609c4-17dd-4c66-8ae9-423ff75efd83', '" + TaskValidationRule.Citizenship.Id + @"')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('af6609c4-17dd-4c66-8ae9-423ff75efd83', '" + TaskValidationRule.Disenfranchised.Id + @"')
                on conflict do nothing;

                insert into ""ElectionType""(""Id"", ""Title"", ""CreatedAt"", ""CreatedBy"")
                values('0db36658-cb07-4ed6-8b53-89c9d0c08f76', 'Kommunal- og Regionsrådsvalg', '" + DateTime.MinValue.AddYears(1970).ToUniversalTime() + @"', '6504020c-3261-41f4-9ba7-ec380f7ad200')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('0db36658-cb07-4ed6-8b53-89c9d0c08f76', '" + TaskValidationRule.Age18.Id + @"')
                on conflict do nothing;

                insert into ""ElectionTypeValidationRule""(""ElectionTypeId"", ""ValidationRuleId"")
                values('0db36658-cb07-4ed6-8b53-89c9d0c08f76', '" + TaskValidationRule.ResidencyMunicipality.Id + @"')
                on conflict do nothing;
                ";
    }
}
