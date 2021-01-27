using System.Collections.Generic;

namespace Altima.Broker.Core
{
    public interface IApplicationBroker
    {
        IList<System.Type> Models { get; }
        IList<System.Type> Services { get; }
        void Build();
    }
}