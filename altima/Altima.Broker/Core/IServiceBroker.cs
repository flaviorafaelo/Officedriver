using Altima.Broker.Business;
using Altima.Broker.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altima.Broker.Core
{
    public interface IServiceBroker<Req, Res, Rep> where Rep : BaseModel
    {
        Res Excecute(Req request, IAsyncRepository<Rep> repository);
    }
}