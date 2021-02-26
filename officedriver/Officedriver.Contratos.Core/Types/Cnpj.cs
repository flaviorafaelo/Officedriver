using Altima.Broker.Business;
using Altima.Broker.Business.Types;
using System;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 19, Mask = "099.999.999/9999-99")]
    public class Cnpj : StringType
    {
        private readonly string _value;
        private Cnpj(string value) : base (value)
        {
            _value = value;
        }

        public static Cnpj Parse(string value)
        {
            if (TryParse(value, out var result))
            {
                return result;
            }
            throw new ArgumentException("CNPJ Inválido");
        }

        public override string ToString()
            => _value;

        public static bool TryParse(string value, out Cnpj cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = value.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (value.Length != 14)
                return false;

            string tempCnpj = value.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            cnpj = value;
            return value.EndsWith(digito);
        }
        public static implicit operator Cnpj(string value)
          => Parse(value);
    }
}
