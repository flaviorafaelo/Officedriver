using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class Model
    {
        public Model(string fullName, string name, IList<IProperty> properties)
        {
            FullName = fullName;
            Name = name;
            Properties = properties;
        }
        public string FullName { get; set; }
        public string Name { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
