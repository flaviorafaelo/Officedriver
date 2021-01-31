using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct DataHora : IDateTimeType
    {
        private readonly string _value;
        private DataHora(string value) => _value = value;
        public static implicit operator DataHora(string value) => new DataHora(value);
    }
}