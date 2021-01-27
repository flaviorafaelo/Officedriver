using System.Linq;
using Altima.Broker.Core;
using Altima.Broker.Business;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.IO;
using Altima.Broker.Business.Types;

public class ApplicationBroker : IApplicationBroker
{
    public IList<Type> Models { get; private set; }

    public IList<Type> Services { get; private set; }

    public IList<Type> Types { get; private set; }

    public void Build()
    {
        //Criar uma função para o item abaixo em utils
        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList<string>();
        foreach (var reference in referencedPaths)
        {
            var currentAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(reference);

            foreach (var model in currentAssembly.GetExportedTypes().Where(x => typeof(BaseModel).IsAssignableFrom(x) && !x.IsAbstract))
            {
                Models.Add(model);
            };

            foreach (var type in currentAssembly.GetExportedTypes().Where(x => typeof(IStringType).IsAssignableFrom(x) && !x.IsAbstract))
            {
                Types.Add(type);
            };
        }
    }

    public ApplicationBroker()
    {
        Models = new List<Type>();
        Types = new List<Type>();
        Build();
    }
}
