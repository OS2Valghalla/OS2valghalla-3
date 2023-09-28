using AutoMapper;
using System.Reflection;

namespace Valghalla.Internal.Infrastructure.Tests
{
    [TestClass]
    public class AutoMapperTests
    {
        private const string ApplicationNamespace = "Valghalla.Internal.Infrastructure";

        [TestMethod]
        public void AutoMapper_Should_Have_ValidConfiguration()
        {
            var configuration = new MapperConfiguration(config => config.AddMaps(Assembly.Load(ApplicationNamespace)));
            configuration.AssertConfigurationIsValid();
        }
    }
}