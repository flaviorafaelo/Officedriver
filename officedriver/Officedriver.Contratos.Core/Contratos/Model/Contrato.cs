using Altima.Broker.Business;
using Officedriver.Contratos.Core.Clientes.Model;
using Officedriver.Contratos.Core.Cooperados.Model;
using Officedriver.Contratos.Core.Types;
using Officedriver.Contratos.Core.Types.Contrato;
using System;
using System.Collections.Generic;
using System.Text;

namespace Officedriver.Contratos.Core.Contratos.Model
{
    public class Contrato: BaseModel
    {
        [ModelAttribute(Required = true)] 
        public Numero Numero { get; set; }
        [ModelAttribute(Required = true)] 
        public Nome Nome { get; set; }
        [ModelAttribute(Required = true)] 
        public Tipo Tipo { get; set; }
        [ModelAttribute(Required = true)] 
        public Cliente Cliente { get; set; }
        [ModelAttribute(Required = true)] 
        public Data Vigencia { get; set; }
        [ModelAttribute(Required = true)] 
        public int DiaFechamento { get; set; }
        public int Intervalo { get; set; } 
        public IEnumerable<Cooperado> Cooperados { get; set; }
        public IEnumerable<Veiculo> Veiculos { get; set; }
        public Template Template { get; set; }
    }
}
