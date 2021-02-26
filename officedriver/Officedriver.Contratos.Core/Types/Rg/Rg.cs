using System;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Rg
{
    public class Rg : ObjectType
    {
        protected Rg() { }

        public Rg(Numero numero, Data expedicao)
        {
            Numero = numero;
            Expedicao = expedicao;
        }
        public Numero Numero { get; set; }
        public Data Expedicao { get; set; }
    }
}
