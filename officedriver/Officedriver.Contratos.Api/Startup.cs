using Altima.Broker.AspNet.Mvc;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Officedriver.Contratos.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAltima(new Options
            {
                ApplicationName = "Officedriver Contratos",
                //StringConnection = "server=191.255.226.139;database=officedriver;User Id=sa;Password=sql@2020;"
                StringConnection = "server=.;database=officedriver;Integrated Security=True;"
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAltima();
        }

    }
}
