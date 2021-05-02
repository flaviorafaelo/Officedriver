using Altima.Broker.Business;
using Altima.Broker.System.Routes;
using Altima.Broker.System.Type;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Altima.Broker.System.Account
{
    public class User: BaseModel
    {
        public Username Username { get; set; }

        public Password Password { get; set; }

        [JsonProperty("rules",ItemConverterType = typeof(StringEnumConverter))] 
        public RulesType Rule { get; set; }

        public Owner Owner { get; set; }
    }
}
