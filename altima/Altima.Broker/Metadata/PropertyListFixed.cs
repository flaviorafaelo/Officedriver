using System.Collections.Generic;

namespace Altima.Broker.Metadata
{
    public class PropertyListFixed : IProperty
    {
        public PropertyListFixed(string name, string type, string caption, bool required, string group, IList<KeyValuePair<string, string>> values)
        {
            Name = name;
            Type = type;
            Caption = caption;
            Required = required;
            Group = group;
            Values = values;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Caption { get; set; }
        public bool Required { get; set; }
        public string Group { get; set; }
        public IList<KeyValuePair<string, string>> Values { get; set; }
    }
}
