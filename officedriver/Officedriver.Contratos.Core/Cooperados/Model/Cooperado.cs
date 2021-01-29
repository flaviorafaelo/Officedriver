using System;
using System.Collections.Generic;
using Altima.Broker.Business;
using Officedriver.Contratos.Core.Contratos.Model;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Cnh;
using Officedriver.Contratos.Core.Types.Conta;
using Officedriver.Contratos.Core.Types.Cooperado;
using Officedriver.Contratos.Core.Types.Endereco;
using Officedriver.Contratos.Core.Types.Rg;
using Officedriver.Contratos.Core.Types.Telefone;

namespace Officedriver.Contratos.Core.Cooperados.Model
{
    public class Cooperado : BaseModel
    {
        [ModelAttribute(Required = true)]
        public Matricula Matricula { get; set; }
        
        [ModelAttribute(Required = true)]
        
        public Nome Nome { get; set; }
        
        public Types.Cooperado.Tipo Tipo { get; set; }
        
        public DataNascimento Nascimento { get; set; }
        
        [ModelAttribute(Required = true)]
        public Rg Rg { get; set; }
        
        [ModelAttribute(Required = true)]
        public Cpf Cpf { get; set; }
        
        [ModelAttribute(Required = true)]
        public Cnh Cnh { get; set; }
        
        public Endereco Endereco { get; set; }
        
        public IList<Contato> Contatos { get; set; }
        
        public Conta Conta { get; set; }
        
        public Imagem Foto { get; set; }
        
        [ModelAttribute(Required = true, Disabled = true)]
        public DataMatricula DataMatricula { get; set; }
        
        [ModelAttribute(Required = true, Disabled = true)]
        public Data DataCadastro { get; set; }
        
        public Data DataDesligamento { get; set; }
        
        public Situacao Situacao { get; set; }
        
        public Telefone Telefone { get; set; }
        
        public Telefone Celular { get; set; }
        
        [ModelAttribute(Required = true)]
        public EMail EMail { get; set; }
        
        public IEnumerable<Imagem> Documentos { get; set; }
        
        public IEnumerable<Contrato> Contratos { get; set; }
        
        public IEnumerable<Veiculo> Veiculos { get; set; }
        //Verificar como funciona o DBSet do Context e implemenar algo parecediro para os serviço

    }
}