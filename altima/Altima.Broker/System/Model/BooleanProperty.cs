using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class BooleanProperty: IProperty
    {
        public BooleanProperty(string name, bool required)
        {
            Type = DataType.Boolean;
            Name = name;
            Required = required;
        }

        public DataType Type { get; private set; }
        public string TypeName { get => "boolean"; }
        public string Name { get; private set; }
        public bool Required { get; private set; }
    }
}
