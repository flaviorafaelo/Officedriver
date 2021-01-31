using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Reembolso;

namespace Officedriver.Contratos.Core.Horas
{
    public class Reembolso
    {
        [ModelAttribute(Required = true)]
        public string Descricao { get; set; }
        [ModelAttribute(Required = true)]
        public TipoDespesa TipoDespesa { get; set; }
        [ModelAttribute(Required = true)]
        public Moeda Valor { get; set; }
    }
}
