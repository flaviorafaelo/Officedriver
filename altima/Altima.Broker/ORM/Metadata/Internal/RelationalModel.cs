using System.Collections.Generic;

namespace Altima.Broker.Relational.Metadata.Internal
{
    public class RelationalModel
    {
        public RelationalModel(IList<Table> tables)
        {
            Tables = tables;
        }

        public IList<Table> Tables { get; set; }       
    }
}
