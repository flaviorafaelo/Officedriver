using Altima.Broker.Relational.Migrations.Operations;

namespace Altima.Broker.Migrations.Operations
{
    public abstract class TableOperation : MigrationOperation
    {
        public virtual string Name { get; set; }

        public virtual string Schema { get; set; }

        public virtual string Comment { get; set; }
    }
}
