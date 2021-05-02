using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.OData.Extensions;

using Altima.Broker.AspNet.Mvc.ApplicationModels;
using Altima.Broker.AspNet.Mvc.ApplicationParts;
using Altima.Broker.Core;
using Altima.Broker.Data;
using Altima.Broker.AspNet.Mvc.Data;
using Altima.Broker.AspNet.Mvc;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Officedriver.Contratos.Core.Cooperados.Model;
using Officedriver.Contratos.Core.Types;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Formatter;
using System.Linq;
using System.Net.Http.Headers;

namespace Microsoft.AspNetCore.Builder
{
    public static class AltimaBuilderExtensions
    {
        public static IApplicationBuilder UseAltima(this IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseODataBatching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Select().Count().Filter().OrderBy().Expand().SkipToken().MaxTop(null);
                endpoints.MapODataRoute(routeName: "api", routePrefix: "api", model: GetEdmModel());

                endpoints.EnableDependencyInjection();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("API SERVER");//colocar classe para vir como pametro
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALTIMA SERVER");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.Migrate();
            }
            return app;
        }

        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            //IApplicationBroker applicationBroker = new ApplicationBroker();
            //foreach(var model in applicationBroker.TypeModels)
            //{
            //    builder.EntitySet<typeof(model)>(nameof(model));
            //
            //}

            builder.EntitySet<Cooperado>(nameof(Cooperado));
            return builder.GetEdmModel();
        }

        public static IServiceCollection AddAltima(this IServiceCollection services, Options options)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = options.ApplicationName, Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            IApplicationBroker applicationBroker = new ApplicationBroker();

            services.AddScoped(typeof(IApplicationBroker), typeof(ApplicationBroker));

            services.AddDbContext<DataContext>(c => c.UseSqlServer(options.StringConnection));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped(typeof(AsyncUserRepository), typeof(AsyncUserRepository));
            
            services.AddSingleton<IApplicationBroker, ApplicationBroker>();


            services.AddMvc(o => o.Conventions.Add(new ControllerConvention()))
                .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new ApplicationFeatureProvider(applicationBroker)));

            services.AddOData();

            AddFormatters(services);

            return services;
        }

        private static void AddFormatters(IServiceCollection services)
        {

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });


        }

    }
}