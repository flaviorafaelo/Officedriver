using System;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Rg
{
    public struct Rg: IObjectType
    {
        public Numero Numero { get; set; }
        public DateTime Expedicao { get; set; }
    }
}
