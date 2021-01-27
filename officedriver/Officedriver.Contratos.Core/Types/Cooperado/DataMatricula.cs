using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Cooperado
{
    public struct DataMatricula : IDateType
    {
        private readonly string _value;
        private DataMatricula(string value) => _value = value;
        public static implicit operator DataMatricula(string value) => new DataMatricula(value);
    }
}
