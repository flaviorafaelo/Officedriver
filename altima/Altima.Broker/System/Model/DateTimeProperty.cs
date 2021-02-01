using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public class DateTimeProperty : IProperty
    {
        public DateTimeProperty(string name, bool required)
        {
            Type = DataType.DateTime;
            Name = name;
            Required = required;
        }

        public DataType Type { get; private set; }
        public string TypeName { get => "dateTime"; }
        public string Name { get; private set; }
        public bool Required { get; private set; }
    }
}
