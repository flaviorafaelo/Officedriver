namespace Altima.Broker.ORM.Storage
{
    public class RelationalCommand
    {
        public RelationalCommand(
         string commandText
            /*IReadOnlyList<IRelationalParameter> parameters*/)
        {
            CommandText = commandText;
            //Parameters = parameters;
        }

        public virtual string CommandText { get; }

        //CreateDbCommand
        //ExecuteNonQueryAsync
    }
}
