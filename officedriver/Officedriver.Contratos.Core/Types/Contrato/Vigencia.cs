using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Contrato
{
    public class Vigencia : ObjectType
    {
        protected Vigencia() { }

        public Data Inicial { get; set; }
        public Data Final { get; set; }
    }
}
