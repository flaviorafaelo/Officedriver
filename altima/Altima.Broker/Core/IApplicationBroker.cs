using System;
using System.Collections.Generic;
using Altima.Broker.System;

namespace Altima.Broker.Core
{
    public interface IApplicationBroker
    {
        IList<Type> TypeModels { get; }
        //IList<Type> BussinesTypes { get; }
        IList<Model> Models { get; }
        IList<View> Views { get; }
    }
}