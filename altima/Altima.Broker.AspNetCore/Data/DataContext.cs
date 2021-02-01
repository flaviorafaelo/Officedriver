using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Altima.Broker.Business;
using Altima.Broker.Core;
using Microsoft.EntityFrameworkCore;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class DataContext : DbContext
    {
        private IApplicationBroker _applicationBroker;
        public DataContext(IApplicationBroker applicationBroker, DbContextOptions<DataContext> options) : base(options)
        {
            _applicationBroker = applicationBroker;
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
            foreach (var type in _applicationBroker.TypeModels)
            {
                modelBuilder.Entity(type);
            }
            
        }
    }
}
