using Altima.Broker.Business;
using Altima.Broker.Business.Types;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Altima.Broker.System.Type
{
    [TypeAttribute(Size = 100)]
    public class Password : StringType
    {
        public static string Crytpo(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(Encoding.ASCII.GetBytes(value)).Select(x => x.ToString("X2")));
            };
        }

        public Password(string value) : base(Crytpo(value))
        {
        }
    }
}
