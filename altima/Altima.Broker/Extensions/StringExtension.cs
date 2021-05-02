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
            return value.Replace(' ', '-').ToLower();// Regex.Replace(value, "[^0-9a-zA-Z]+", "").Replace(' ','-').ToLower();
        }
    }
}
