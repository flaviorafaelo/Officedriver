using System.Collections.Generic;

namespace Altima.Broker.Relational.Metadata
{
    public class Table
    {
        public Table(string schema, string name, IList<Column> columns)
        {
            Schema = schema;
            Name = name;
            Columns = columns;
        }

        public string Schema { get; set; }
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }
    }
}
