using System;
using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Rg
{
    public struct Expedicao : IDateType
    {
        private readonly DateTime _value;
        private Expedicao(DateTime value) => _value = value;
        public static implicit operator Expedicao(DateTime value) => new Expedicao(value);
    }
}
