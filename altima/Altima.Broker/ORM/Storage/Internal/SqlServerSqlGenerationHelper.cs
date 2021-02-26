namespace Altima.Broker.Relational.Storage.Internal
{
    public class SqlServerSqlGenerationHelper
    {
        public virtual string StatementTerminator => ";";

        public string EscapeIdentifier(string identifier) => (nameof(identifier)).Replace("]", "]]");

        public string DelimitIdentifier(string identifier) => $"[{EscapeIdentifier(nameof(identifier))}]"; 
    }
}
