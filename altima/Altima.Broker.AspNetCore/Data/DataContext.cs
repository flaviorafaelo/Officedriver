using System;
using System.Reflection;
using Altima.Broker.Core;
using Altima.Broker.Metadata.Generator;
using Altima.Broker.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Officedriver.Contratos.Core.Clientes.Model;
using Officedriver.Contratos.Core.Cooperados.Model;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class DataContext : DbContext
    {
        private readonly IApplicationBroker _applicationBroker;
        public DataContext(IApplicationBroker applicationBroker, DbContextOptions<DataContext> options) : base(options)
        {
            _applicationBroker = applicationBroker;
        }

     //   public DbSet<Cooperado> Cooperados { get; set; }
     //   public DbSet<Cliente> Clientes { get; set; }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            RegisterAllModels(builder);
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void RegisterProperty(PropertyBuilder propertyBuilder)
        {
            //    case Business.Types.DataType.List:
            //        builder.Property(propertyName).HasColumnName(propertyName).IsRequired(true);
            //break;

            //propertyBuilder.
        }

        private void RegisterOwnsOneProperty(string modelName, string columnName, IProperty property, OwnedNavigationBuilder ownedNavigationBuilder)
        {
            switch (property.Type)
            {
                case Business.Types.DataType.String:
                    var propertySize = (property as StringProperty).Size;
                    if (propertySize == 0)
                        throw new ArgumentException($"StringType must have size {property.Name} in {modelName}");
                    ownedNavigationBuilder.Property("Value").HasMaxLength(propertySize).HasColumnName(columnName);
                    break;

                case Business.Types.DataType.Object:
                    var objectProperty = property as ObjectProperty;
                    foreach (var objProp in objectProperty.Properties)
                    {
                        var propertyObjFullName = ownedNavigationBuilder.OwnedEntityType.ClrType.GetProperty(objProp.Name).PropertyType.FullName;
                        ownedNavigationBuilder.OwnsOne(propertyObjFullName, objProp.Name, b2 =>
                        {
                            RegisterOwnsOneProperty(modelName, string.Concat(columnName, objProp.Name), objProp, b2);
                        });
                    }
                    break;

                case Business.Types.DataType.Record:
                    var recordProperty = property as RecordProperty;
                    foreach (var objProp in recordProperty.Properties)
                    {
                        var propertyObjFullName = ownedNavigationBuilder.OwnedEntityType.ClrType.GetProperty(objProp.Name).PropertyType.FullName;
                        ownedNavigationBuilder.OwnsOne(propertyObjFullName, objProp.Name, b2 =>
                        {
                            RegisterOwnsOneProperty(modelName, objProp.Name, objProp, b2);
                        });
                    }
                    break;

                default:
                    ownedNavigationBuilder.Property("Value").HasColumnName(columnName);
                    break;
            }
        }

        private void RegisterModel(Type type, EntityTypeBuilder builder)
        {
            builder.HasKey("Id");
            Model atmModel = ModelGenerator.Create(type);
            foreach (var atmProperty in atmModel.Properties)
            {
                var modelName = atmModel.Name;
                var propertyName = atmProperty.Name;
                var propertyFullName = type.GetProperty(propertyName).PropertyType.FullName;

                Console.WriteLine(propertyFullName);
                switch (atmProperty.Type)
                {

                    case Business.Types.DataType.List:
                        builder.Property(propertyName).HasColumnName(propertyName).IsRequired(true);
                        break;

                    case Business.Types.DataType.Record:
                        builder.OwnsMany(propertyFullName, propertyName, b1 =>
                        {
                            b1.ToTable(string.Concat(modelName,propertyName));
                            RegisterOwnsOneProperty(modelName, propertyName, atmProperty, b1);
                        });
                        break;

                    default:
                        builder.OwnsOne(propertyFullName, propertyName, b1 =>
                        {
                            RegisterOwnsOneProperty(modelName, propertyName, atmProperty, b1);
                        });
                        break;
                }
            }
        }

        private void RegisterAllModels(ModelBuilder modelBuilder)
        {
            foreach (var type in _applicationBroker.TypeModels)
            {
                Console.WriteLine("BUILD OBJECT =" + type.Name);
                var builder = modelBuilder.Entity(type);
                RegisterModel(type, builder);
            }
        }
    }
}
