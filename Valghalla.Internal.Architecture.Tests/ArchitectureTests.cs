using NetArchTest.Rules;

namespace Architecture.Tests
{
    [TestClass]
    public class ArchitectureTests
    {
        private const string InfrastructureNamespace = "Valghalla.Internal.Infrastructure";
        private const string APINamespace = "Valghalla.Internal.API";

        [TestMethod]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            var assembly = typeof(Valghalla.Internal.Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                APINamespace
            };

            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            var assembly = typeof(Valghalla.Internal.Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                APINamespace,
                //ApplicationNamespace
            };

            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Controllers_Should_Have_DependencyOnMediatR()
        {
            var assembly = typeof(Valghalla.Internal.API.AssemblyReference).Assembly;

            var result = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void Validators_Should_Be_Public()
        {
            var assembly = typeof(Valghalla.Internal.Application.AssemblyReference).Assembly;

            var result = Types.InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Validator")
                .Should()
                .BePublic()
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }
    }
}