using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests
{
    internal class ApiApplication : WebApplicationFactory<Program>
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
            });

            return base.CreateHost(builder);
        }
    }
}
