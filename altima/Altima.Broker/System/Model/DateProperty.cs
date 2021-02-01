using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class DateProperty : IProperty
    {
        public DateProperty(string name, bool required)
        {
            Type = DataType.Date;
            Name = name;
            Required = required;
        }

        public DataType Type { get; private set; }
        public string TypeName { get => "date"; }
        public string Name { get; private set; }
        public bool Required { get; private set; }
    }
}
