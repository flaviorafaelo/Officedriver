using System;

namespace Altima.Broker.Business
{
    //TODO: Verificar no projeto seu chef como foi gerado o numero numero do pedido
    public abstract class BaseModel
    {
        protected virtual long Id { get; set; }

        protected BaseModel()
        {
           // Random rnd = new Random();//TEMPORAAIO
          //  Id = rnd.Next();
        }
    }
}
