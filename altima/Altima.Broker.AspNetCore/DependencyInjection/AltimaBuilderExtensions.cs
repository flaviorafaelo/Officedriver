using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Altima.Broker.AspNet.Mvc.ApplicationModels;
using Altima.Broker.AspNet.Mvc.ApplicationParts;
using Altima.Broker.Core;
using Altima.Broker.Data;
using Altima.Broker.AspNet.Mvc.Data;
using Microsoft.Extensions.Configuration;
using Altima.Broker.AspNet.Mvc;

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALTIMA SERVER");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("API SERVER");//colocar classe para vir como pametro
                });
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.Migrate();
            }

            return app;
        }

        public static IServiceCollection AddAltima(this IServiceCollection services, Options options)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<AddCommonParameOperationFilter>();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = options.ApplicationName, Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            IApplicationBroker applicationBroker = new ApplicationBroker();

            services.AddScoped(typeof(IApplicationBroker), typeof(ApplicationBroker));

            services.AddDbContext<DataContext>(c => c.UseSqlServer(options.StringConnection));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            services.AddSingleton<IApplicationBroker, ApplicationBroker>();

            services.AddMvcCore(o => o.Conventions.Add(new ControllerConvention())).
                           ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new ApplicationFeatureProvider(applicationBroker)));
                         //ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new RemoteControllerFeatureProvider()));

            return services;
        }
    }
}