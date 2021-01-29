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
            cpf = "";

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            value = value.Trim().Replace(".", "").Replace("-", "");
            if (value.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == value)
                    return false;

            string tempCpf = value.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            cpf = value;
            return value.EndsWith(digito);
        }
        public static implicit operator Cpf(string value)
          => Parse(value);
    }
}

//criar na Model o tipo chave <TId>
