using System.Text;

using Microsoft.Extensions.Options;

using SF1520;

using Valghalla.Application.Configuration;
using Valghalla.Application.CPR;
using Valghalla.Application.Exceptions;
using Valghalla.Application.Secret;

namespace Valghalla.Integration.CPR
{
    internal class CPRService : ICPRService
    {
        private PersonBaseDataExtendedPortTypeClient? _client = null;

        private readonly ISecretService secretService;
        private readonly AppConfiguration appConfiguration;
        private readonly SecretConfiguration secretConfiguration;

        public CPRService(ISecretService secretService, AppConfiguration appConfiguration, IOptions<SecretConfiguration> options)
        {
            this.secretService = secretService;
            this.appConfiguration = appConfiguration;
            this.secretConfiguration = options.Value;
        }

        public async Task<CprPersonInfo> ExecuteAsync(string cpr)
        {
            if (!CprValidator.IsCvrValid(appConfiguration.Cvr))
            {
                throw new CvrInputInvalidException(appConfiguration.Cvr);
            }

            if (!CprValidator.IsCprValid(cpr))
            {
                throw new CprInputInvalidException(cpr);
            }

            PersonLookupRequestType request = new()
            {
                PNR = cpr,
                AuthorityContext = new AuthorityContextType()
                {
                    MunicipalityCVR = appConfiguration.Cvr
                }
            };

            var client = await CreateClientAsync();
            var komkodDict = await secretService.GetKomkodDictionaryAsync(default);

            PersonLookupResponse response = await client.PersonLookupAsync(request);
            return Convert(response, komkodDict);
        }

        private async Task<PersonBaseDataExtendedPortTypeClient> CreateClientAsync()
        {
            if (_client != null) return await Task.FromResult(_client);

            var config = await secretService.GetCprConfigurationAsync(default);
            var certFilePath = Path.Combine(AppContext.BaseDirectory, Path.GetDirectoryName(secretConfiguration.Path)!, config.SigningCertificateFile);

            if (string.IsNullOrEmpty(config.Thumbprint))
            {
                _client = new PersonBaseDataExtendedPortTypeClient(
                    config.ServiceUrl,
                    certFilePath,
                    config.SigningCertificatePassword);
            }
            else
            {
                var certPath = string.Format(config.CertPath, config.Thumbprint);
                _client = new PersonBaseDataExtendedPortTypeClient(config.ServiceUrl, certPath);
            }

            return _client;
        }

        private static CprPersonInfo Convert(PersonLookupResponse response, IDictionary<string, string> komkodDict)
        {
            var cpr = response.PersonLookupResponse1.persondata.personnummer;
            var firstName = response.PersonLookupResponse1.persondata.navn.fornavn;

            if (!string.IsNullOrEmpty(response.PersonLookupResponse1.persondata.navn.mellemnavn))
            {
                firstName = firstName + " " + response.PersonLookupResponse1.persondata.navn.mellemnavn;
            }

            var lastName = response.PersonLookupResponse1.persondata.navn.efternavn;
            var countryCode = response.PersonLookupResponse1.persondata.statsborgerskab.landekode;
            var countryName = response.PersonLookupResponse1.persondata.statsborgerskab.landekodeMyndighedsnavn;

            string? street;
            string? postalCode;
            string? city = null;
            string? municipalityName = null;
            string? municipalityCode = null;
            bool protectedAddress = false;

            if (response.PersonLookupResponse1.adresse?.aktuelAdresse?.standardadresse != null)
            {
                protectedAddress = response.PersonLookupResponse1.persondata.adressebeskyttelse?.beskyttet ?? false;

                street = response.PersonLookupResponse1.adresse.aktuelAdresse.standardadresse;
                postalCode = response.PersonLookupResponse1.adresse.aktuelAdresse.postnummer;
                city = response.PersonLookupResponse1.adresse.aktuelAdresse.postdistrikt;

                var komkod = response.PersonLookupResponse1.adresse.aktuelAdresse.kommunekode.PadLeft(4, '0');

                municipalityCode = komkod;
                municipalityName = komkodDict.ContainsKey(komkod) ? komkodDict[komkod] : null;
            }
            else
            {
                street = response.PersonLookupResponse1.adresse.udrejseoplysninger.udlandsadresse1;

                var postalCodeBuilder = new StringBuilder();
                var cityBuilder = new StringBuilder();

                foreach (var c in response.PersonLookupResponse1.adresse.udrejseoplysninger.udlandsadresse2.ToCharArray())
                {
                    if (c >= '0' && c <= '9' && cityBuilder.Length == 0)
                    {
                        postalCodeBuilder.Append(c);
                    }
                    else if (cityBuilder.Length > 0 || c != ' ')
                    {
                        cityBuilder.Append(c);
                    }
                }

                postalCode = postalCodeBuilder.ToString();
                //city = cityBuilder.ToString();
                //country = response.PersonLookupResponse1.adresse.udrejseoplysninger.udlandsadresse3;
            }

            var birthdate = response.PersonLookupResponse1.persondata.foedselsdato.dato.ToUniversalTime();
            var age = int.Parse(response.PersonLookupResponse1.persondata.alder);

            //DateTime? citizenship = null;

            //if (response.PersonLookupResponse1.persondata.statsborgerskab.statsborgerskabDato != null)
            //{
            //    citizenship = response.PersonLookupResponse1.persondata.statsborgerskab.statsborgerskabDato.dato.ToUniversalTime();
            //}

            var isDead = false;

            if (response.PersonLookupResponse1.persondata?.status?.status != null)
            {
                // 90 is "Død", 70 is "Bortkommet"
                if (90 == response.PersonLookupResponse1.persondata.status.status ||
                    70 == response.PersonLookupResponse1.persondata.status.status)
                {
                    isDead = true;
                }
            }

            var disenfranchised = false;

            if (response.PersonLookupResponse1.persondata?.umyndiggoerelse?.umyndiggjort == true)
            {
                disenfranchised = true;
            }

            var exemptDigitalPost = false;

            if (response.PersonLookupResponse1.persondata.tilmeldtDigitalpost != null)
            {
                if (response.PersonLookupResponse1.persondata.tilmeldtDigitalpost == false)
                {
                    exemptDigitalPost = true;
                }
            }

            return new()
            {
                Cpr = cpr,
                FirstName = firstName,
                LastName = lastName,
                Address = new()
                {
                    Street = street,
                    City = city,
                    PostalCode = postalCode,
                    ProtectedAddress = protectedAddress
                },
                Country = new()
                {
                    Code = countryCode,
                    Name = countryName,
                },
                Municipality = new()
                {
                    Code = municipalityCode,
                    Name = municipalityName,
                },
                Age = age,
                Birthdate = birthdate,
                CountryCode = countryCode,
                CountryName = countryName,
                Deceased = isDead,
                Disenfranchised = disenfranchised,
                ExemptDigitalPost = exemptDigitalPost,
            };
        }
    }
}
