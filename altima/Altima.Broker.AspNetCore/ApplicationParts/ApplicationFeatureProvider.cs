using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Altima.Broker.AspNet.Mvc.Controllers;
using Altima.Broker.Core;
using System.Linq;
using Altima.Broker.Business;

namespace Altima.Broker.AspNet.Mvc.ApplicationParts
{
    public class ApplicationFeatureProvider: IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IApplicationBroker ApplicationBroker;

        public ApplicationFeatureProvider(IApplicationBroker applicationBroker)
        {
            ApplicationBroker = applicationBroker;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var model in ApplicationBroker.TypeModels)
            {
                feature.Controllers.Add(typeof(ModelController<>).MakeGenericType(model).GetTypeInfo());
            }
        }
    }
}