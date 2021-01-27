using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 5000)]
    public struct Imagem : IImageType
    {
        private readonly string _value;
        private Imagem(string value) => _value = value;
        public static implicit operator Imagem(string value) => new Imagem(value);
    }
}
