using Altima.Broker.Business.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Officedriver.Contratos.Core.Types.Contrato
{
    public struct Vigencia : IObjectType
    {
        public DateTime Inicial { get; set; }
        public DateTime Final { get; set; }
    }
}
