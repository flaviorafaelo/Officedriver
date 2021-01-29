using System;
using System.Collections.Generic;
using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Veiculo;

namespace Officedriver.Contratos.Core.Cooperados.Model
{
    public class Veiculo: BaseModel
    {
        [ModelAttribute(Required = true)]
        public Nome Proprietario { get; set; }
        [ModelAttribute(Required = true)]
        public int Fabrincante { get; set; }
        [ModelAttribute(Required = true)]
        public int Modelo { get; set; }
        [ModelAttribute(Required = true)]
        public Placa Placa { get; set; }
        [ModelAttribute(Required = true)]
        public int AnoFabricacao { get; set; }
        [ModelAttribute(Required = true)]
        public int AnoModelo { get; set; }
        public Tipo Tipo { get; set; }
        public string Potencia { get; set; }
        public Combustivel TipoCombustivel { get; set; }
        public string Cor { get; set; }
        public string Rastreador { get; set; }
        public string NumeroRastreador { get; set; }
        public Data? VencimentoSeguroAPP { get; set; }
        public IEnumerable<Imagem> Documentos { get; set; }
        public IEnumerable<Contato> Contatos { get; set; }
    }
}
