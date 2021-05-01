using System.Collections.Generic;

namespace Altima.Broker.System.Routes
{
    public class Param {
        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }

    public class Service
    {
        public Service(string name, IList<Param> @params)
        {
            Name = name;
            Params = @params;
        }

        public string Name { get; }
        public IList<Param> Params { get; }
    }
}