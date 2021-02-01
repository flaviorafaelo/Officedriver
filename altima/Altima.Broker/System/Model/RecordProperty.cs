
using Altima.Broker.Business.Types;
using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class RecordProperty : IProperty
    {
        public RecordProperty(string name, bool required, IList<IProperty> properties)
        {
            Type = DataType.Record;
            Name = name;
            Required = required;
            Properties = properties;
        }

        public string Name { get; set; }
        public DataType Type { get; set; }
        public string TypeName { get => "record"; }
        public bool Required { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
