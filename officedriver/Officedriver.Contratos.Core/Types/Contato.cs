using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct Contato: IObjectType
    {
        public Nome Nome { get; set; }
        public Telefone.Telefone Telefone { get; set; }
        public EMail EMail { get; set; }
    }
}
