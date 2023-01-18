using AutoFixture;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests
{
    internal class ApiApplication<T>: WebApplicationFactory<T> where T: class
    {
        private readonly string _environment;

        public ApiApplication(string environment = "Development")
        {
            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);

            builder.ConfigureServices(services =>
            {
                services.AddSingleton<Fixture>();
                services.AddSingleton<TestDataManager>();
            });

            return base.CreateHost(builder);
        }
    }
}
