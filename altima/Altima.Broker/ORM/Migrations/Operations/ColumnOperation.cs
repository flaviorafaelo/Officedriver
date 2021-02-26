using Altima.Broker.Relational.Migrations.Operations;

namespace Altima.Broker.Migrations.Operations
{
    public class ColumnOperation : MigrationOperation
    {
        public virtual string Schema { get; set; }
        public virtual string Table { get; set; }
        public virtual string Name { get; set; }
        public virtual string ColumnType { get; set; }
        public virtual bool IsNullable { get; set; }
        public virtual int? MaxLength { get; set; }
        public virtual object DefaultValue { get; set; }
        public virtual string Comment { get; set; }
        public virtual int? Precision { get; set; }
        public virtual int? Scale { get; set; }
    }
}
