using System;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Cnh
{
    public struct Validade : IDateType
    {
        private readonly DateTime _value;
        private Validade(DateTime value) => _value = value;
        public static implicit operator Validade(DateTime value) => new Validade(value);
    }
}
