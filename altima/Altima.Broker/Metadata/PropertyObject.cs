using System.Collections.Generic;

namespace Altima.Broker.Metadata
{
    public class PropertyObject : IProperty
    {
        public PropertyObject(string name, string type, string caption, bool required, string group, IList<IProperty> properties)
        {
            Name = name;
            Type = type;
            Caption = caption;
            Required = required;
            Group = group;
            Properties = properties;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Caption { get; set; }
        public bool Required { get; set; }
        public string Group { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
