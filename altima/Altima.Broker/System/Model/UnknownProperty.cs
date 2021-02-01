using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class UnknownProperty : IProperty
    {
        public UnknownProperty(string name, bool required)
        {
            Type = DataType.Unknown;
            Name = name;
            Required = required;
        }

        public string Name { get; set; }
        public string TypeName { get => "unknown"; }
        public DataType Type { get; set; }
        public bool Required { get; set; }
    }
}
