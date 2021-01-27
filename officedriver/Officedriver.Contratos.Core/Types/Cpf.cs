using System;
using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 14, Mask = "999.999.999-99")]
    public struct Cpf: IStringType
    {
        private readonly string _value;
        private Cpf(string value) 
            => _value = value; 

        public static Cpf Parse(string value)
        {
            if (TryParse(value, out var result))
            {
                return result;
            }
            throw new ArgumentException("CPF Inválido");
        }

        public override string ToString()
            => _value;

        public static bool TryParse(string value, out Cpf cpf)
        {
            //.. validation Logic 
            cpf = value;
            return true;
        }
        public static implicit operator Cpf(string value)
          => Parse(value);
    }
}

//criar na Model o tipo chave <TId>
