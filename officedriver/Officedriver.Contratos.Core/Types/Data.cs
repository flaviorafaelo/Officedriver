using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct Data : IDateType
    {
        private readonly string _value;
        private Data(string value) => _value = value;
        public static implicit operator Data(string value) => new Data(value);
    }
}
