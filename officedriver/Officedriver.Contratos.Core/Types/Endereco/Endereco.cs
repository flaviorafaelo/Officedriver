using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    public struct Endereco : IObjectType
    {   
        //criar evento de validação do campo se ele é valido
        public Cep Cep { get; set; } 
        public Logradouro Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
    }
}