using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class NumericProperty : IProperty
    {
        public NumericProperty(string name, byte precision, bool required)
        {
            Type = DataType.Numeric;
            Name = name;
            Precision = precision;
            Required = required;
        }

        public DataType Type { get; private set; }
        public string TypeName { get => "numeric"; }
        public string Name { get; private set; }
        public byte Precision { get; set; }
        public bool Required { get; private set; }
    }
}