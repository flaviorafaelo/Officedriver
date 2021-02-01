using System.Collections.Generic;
using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class ObjectProperty : IProperty
    {
        public ObjectProperty(string name, bool required, IList<IProperty> properties)
        {
            Type = DataType.Object;
            Name = name;
            Required = required;
            Properties = properties;
        }

        public string Name { get; set; }
        public string TypeName { get => "object"; }
        public DataType Type { get; set; }
        public bool Required { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
