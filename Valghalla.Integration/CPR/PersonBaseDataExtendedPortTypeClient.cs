using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace SF1520
{
    public partial class PersonBaseDataExtendedPortTypeClient : ClientBase<PersonBaseDataExtendedPortType>, PersonBaseDataExtendedPortType
    {
        public PersonBaseDataExtendedPortTypeClient(string endpointUrl, string certPath, string certPassword) : base(GetBindingForEndpoint(), GetEndpointAddress(endpointUrl))
        {
            //this.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(fileName: certPath, password: SecureStringUtil.ConvertToSecureString(certPassword));
            this.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(fileName: certPath, password: certPassword);
            // Disable revocation checking
            this.ClientCredentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public PersonBaseDataExtendedPortTypeClient(string endpointUrl, string certPath) : base(GetBindingForEndpoint(), GetEndpointAddress(endpointUrl))
        {
            var bytes = File.ReadAllBytes(certPath);
            var x509Certificate = new X509Certificate2(bytes);

            //this.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(fileName: certPath, password: SecureStringUtil.ConvertToSecureString(certPassword));
            this.ClientCredentials.ClientCertificate.Certificate = x509Certificate;
            // Disable revocation checking
            this.ClientCredentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint()
        {
            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.OpenTimeout = new TimeSpan(0, 0, 30);
            binding.CloseTimeout = new TimeSpan(0, 0, 30);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 30);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = new TimeSpan(0, 0, 30);

            return binding;
        }

        private static EndpointAddress GetEndpointAddress(string endpointUrl)
        {
            return new EndpointAddress(endpointUrl);
        }

    }
}
