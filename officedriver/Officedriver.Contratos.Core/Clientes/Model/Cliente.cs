using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Endereco;

namespace Officedriver.Contratos.Core.Clientes.Model
{
    public class Cliente: BaseModel
    {
        public Nome Nome { get; set; }
        public Endereco Endereco { get; set; }
    }
}
