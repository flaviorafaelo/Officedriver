using System.Collections.Generic;
using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Cnh;
using Officedriver.Contratos.Core.Types.Conta;
using Officedriver.Contratos.Core.Types.Endereco;
using Officedriver.Contratos.Core.Types.Rg;

namespace Officedriver.Contratos.Core.Cooperados.Model
{
    public class Cooperado : BaseModel
    {
        [ModelAttribute(Required = true)]
        public Matricula Matricula { get; set; }
        public Nome Nome { get; set; }
        public Tipo Tipo { get; set; }
        public DataNascimento Nascimento { get; set; }
        public Rg Rg { get; set; }
        public Cpf Cpf { get; set; }
        public Cnh Cnh { get; set; }
        public Endereco Endereco { get; set; }
        public IList<Contato> Contatos { get; set; }
        public Conta Conta { get; set; }

        //Verificar como funciona o DBSet do Context e implemenar algo parecediro para os serviço

    }
}