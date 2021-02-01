using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Altima.Broker.Business;
using Microsoft.EntityFrameworkCore;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //
        }

  

        //public DbSet<Cooperado> Cooperados { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            RegisterAllModels(builder);
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void RegisterAllModels(ModelBuilder modelBuilder)
        {
            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList<string>();
            foreach (var reference in referencedPaths)
            {
                var currentAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(reference);

                foreach (var entity in currentAssembly.GetExportedTypes().Where(x => typeof(BaseModel).IsAssignableFrom(x) && !x.IsAbstract))
                {
                    modelBuilder.Entity(entity);
                };
            }
        }
    }
}
