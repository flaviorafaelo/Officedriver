using Altima.Broker.Migrations.Operations;

namespace Altima.Broker.Relational.Migrations.Operations.Builders
{
    public class CreateTableBuilder : OperationBuilder<CreateTableOperation>
    {
        public CreateTableBuilder(CreateTableOperation operation)
        : base(operation)
        {

        }

        public virtual OperationBuilder<AddPrimaryKeyOperation> PrimaryKey(
            string name,
            string[] columns)
        {
            var operation = new AddPrimaryKeyOperation
            {
                Schema = Operation.Schema,
                Table = Operation.Name,
                Name = name,
                Columns = columns
            };
            Operation.PrimaryKey = operation;
            return new OperationBuilder<AddPrimaryKeyOperation>(operation);
        }
    }
}
