using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class StringProperty : IProperty
    {
        public StringProperty(string name, int size, bool required)
        {
            Type = DataType.String;
            Name = name;
            Size = size;
            Required = required;
        }
        public DataType Type { get; private set; }
        public string TypeName { get => "string"; }
        public string Name { get; private set; }
        public int Size { get; private set; }
        public bool Required { get; private set; }
    }
}
