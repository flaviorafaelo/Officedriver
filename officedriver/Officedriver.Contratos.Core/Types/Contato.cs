using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public class Contato : ObjectType
    {
        protected Contato() { }

        public Nome Nome { get; set; }
        public Telefone.Telefone Telefone { get; set; }
        public EMail EMail { get; set; }
    }
}
