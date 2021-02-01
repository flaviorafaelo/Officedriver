using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class IntegerProperty : IProperty
    {
        public IntegerProperty(string name, bool required)
        {
            Type = DataType.Integer;
            Name = name;
            Required = required;
        }

        public DataType Type { get; }
        public string TypeName { get => "integer"; }
        public string Name { get; }
        public bool Required { get; }
    }
}
