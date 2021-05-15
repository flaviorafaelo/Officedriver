using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Altima.Broker.Extensions
{
    public static class StringExtension
    {
        public static string FormatToUrl(this string value) 
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var acentos = "çÇáéíóúýÁÉÍÓÚÝàèìòùÀÈÌÒÙãõñäëïöüÿÄËÏÖÜÃÕÑâêîôûÂÊÎÔÛ";
            var semacetos = "cCaeiouyAEIOUYaeiouAEIOUaonaeiouyAEIOUAONaeiouAEIOU";
            for (int i = 0; i < acentos.Length; i++)
                value = value.Replace(acentos[i], semacetos[i]);

            return value.Replace(" ", "-").ToLower();
        }
    }
}
