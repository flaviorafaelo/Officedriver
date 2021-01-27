using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 200, Validation = @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$")]
    public struct EMail : IStringType
    {
        private readonly string _value;
        private EMail(string value) => _value = value;
        public static implicit operator EMail(string value) => new EMail(value);
    }
}