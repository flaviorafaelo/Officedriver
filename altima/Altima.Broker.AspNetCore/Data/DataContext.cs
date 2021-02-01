using System.Reflection;
using Altima.Broker.Core;
using Microsoft.EntityFrameworkCore;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class DataContext : DbContext
    {
        private readonly IApplicationBroker _applicationBroker;
        public DataContext(IApplicationBroker applicationBroker, DbContextOptions<DataContext> options) : base(options)
        {
            _applicationBroker = applicationBroker;
        }

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
