using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Altima.Broker.AspNet.Mvc.ApplicationModels
{
    public class ControllerConvention : IControllerModelConvention
    {
       public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var genericType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = genericType.Name;
            }
        }
    }
}
