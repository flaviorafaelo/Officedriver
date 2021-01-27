using Altima.Broker.Business;
using Altima.Broker.Business.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Officedriver.Contratos.Core.Types.Veiculo
{
    [TypeAttribute(Size = 7, Validation = @"/[A-Z]{3}[0-9][0-9A-Z][0-9]{2}/")]
    public struct Placa : IStringType
    {
        private readonly string _value;
        private Placa(string value) => _value = value;
        public static implicit operator Placa(string value) => new Placa(value);
    }
}
