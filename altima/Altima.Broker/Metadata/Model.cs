using System.Collections.Generic;

namespace Altima.Broker.Metadata
{
    public class Model
    {
        public Model(string name, IList<IProperty> properties)
        {
            Name = name;
            Properties = properties;
        }

        public string Name { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
