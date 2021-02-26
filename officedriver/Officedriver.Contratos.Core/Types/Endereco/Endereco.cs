using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    public class Endereco : ObjectType
    {
        protected Endereco() { }

        //criar evento de validação do campo se ele é valido
        public Cep Cep { get; set; } 
        public Logradouro Logradouro { get; set; }
        public Numero Numero { get; set; }
        public Complemento Complemento { get; set; }
        public Bairro Bairro { get; set; }
        public Estado Estado { get; set; }
        public Cidade Cidade { get; set; }
    }
}