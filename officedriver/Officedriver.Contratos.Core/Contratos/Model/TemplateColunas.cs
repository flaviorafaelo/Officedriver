using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;

namespace Officedriver.Contratos.Core.Contratos.Model
{
    class TemplateColuna: BaseModel
    {
        public Nome Nome { get; set; }
        public string Descricao { get; set; }
    }
}
