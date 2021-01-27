using Altima.Broker.Business.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Officedriver.Contratos.Core.Types.Contrato
{
    public struct DataVigencia : IDateType
    {
        private readonly string _value;
        private DataVigencia(string value) => _value = value;
        public static implicit operator DataVigencia(string value) => new DataVigencia(value);
    }
}
