using Altima.Broker.Business;
using Officedriver.Contratos.Core.Contratos.Model;
using Officedriver.Contratos.Core.Cooperados.Model;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Cliente;
using Officedriver.Contratos.Core.Types.Endereco;
using System.Collections.Generic;

namespace Officedriver.Contratos.Core.Clientes.Model
{
    public class Cliente : BaseModel
    {
        public Tipo TipoPessoa { get; set; }
        [ModelAttribute(Required = true)]
        public Nome Nome { get; set; }
        [ModelAttribute(Required = true)]
        public Cnpj Cnpj { get; set; }
        // public Imagem Logo { get; set; }
        public Endereco Endereco { get; set; }
       // public IEnumerable<Contato> Contatos { get; set; }
        // public IEnumerable<Veiculo> Veiculos { get; set; }
        //public IEnumerable<Contrato> Contratos { get; set; }
    }
}
