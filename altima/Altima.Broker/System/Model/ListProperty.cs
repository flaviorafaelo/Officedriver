using Altima.Broker.Business.Types;
using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class ListProperty : IProperty
    {
        public ListProperty(string name, bool required, IList<KeyValuePair<string, string>> values)
        {
            Type = DataType.List;
            Name = name;
            Required = required;
            Values = values;
        }

        public DataType Type { get; set; }
        public string TypeName { get => "list"; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public IList<KeyValuePair<string, string>> Values { get; set; }
    }
}
