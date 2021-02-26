namespace Altima.Broker.Relational.Migrations.Operations.Builders
{
    public class OperationBuilder<TOperation>
    {
        public OperationBuilder(TOperation operation)
        {
            Operation = operation;
        }
        protected virtual TOperation Operation { get; }
    }
}
