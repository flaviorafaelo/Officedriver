using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Officedriver.Contratos.Core.Cooperados.Model;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Cooperado> Cooperados { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        }
    }
}
