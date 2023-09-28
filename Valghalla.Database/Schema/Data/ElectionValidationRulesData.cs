using Valghalla.Application.TaskValidation;

namespace Valghalla.Database.Schema.Data
{
    internal class ElectionValidationRulesData
    {
        internal static string InitData()
            => $@"
                insert into ""ElectionValidationRule"" values('" + TaskValidationRule.Age18.Id + @"', 'Age18')
                on conflict do nothing;
                insert into ""ElectionValidationRule"" values('" + TaskValidationRule.ResidencyMunicipality.Id + @"', 'MunicipalRequirement')
                on conflict do nothing;
                insert into ""ElectionValidationRule"" values('" + TaskValidationRule.Citizenship.Id + @"', 'DanishCitizen')
                on conflict do nothing;
                insert into ""ElectionValidationRule"" values('" + TaskValidationRule.Disenfranchised.Id + @"', 'Disempowered')
                on conflict do nothing;
                ";
    }
}
