using System.Collections.Generic;

namespace Altima.Broker.Migrations.Operations
{
    public class CreateTableOperation: TableOperation
    {
        public virtual List<AddColumnOperation> Columns { get; } = new List<AddColumnOperation>();
        public virtual AddPrimaryKeyOperation PrimaryKey { get; set; }
    }
}
