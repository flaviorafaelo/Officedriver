using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using Altima.Broker.System;
using Altima.Broker.Core;
using Altima.Broker.Business;
using System.Reflection;
using Altima.Broker.Metadata.Generator;

public class ApplicationBroker : IApplicationBroker
{
    private readonly IList<Assembly> _assemblies;
    public IList<Type> TypeModels { get; private set; }
    public IList<Model> Models { get; private set; }
    public IList<View> Views { get; private set; }

    private IList<Assembly> GetAllAssembliesSystem()
    {
        var assemblies = new List<Assembly>();
        var referencedPaths = Workaround.GetFiles("*.dll");
        foreach (var reference in referencedPaths)
            assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(reference));
        return assemblies;
    }

    private IList<Type> FilterTypes(IList<Assembly> assemblies, Type type)
    {
        IList<Type> models = new List<Type>();

        foreach (var assembly in assemblies)
        {
            foreach (var model in assembly.GetExportedTypes().Where(x => type.IsAssignableFrom(x) && !x.IsAbstract))
            {
                if (model.Name.Contains("Cooperado") || model.Name.Contains("ClienteX"))
                        models.Add(model);
            };
        }
        return models;
    }

    private IList<Model> CreateModels(IList<Type> typeModels)
    {
        IList<Model> models = new List<Model>();
        foreach (var typeModel in typeModels)
           models.Add(ModelGenerator.Create(typeModel));
        return models;
    }

    private IList<View> CreateViews(IList<Model> models)
    {
        IList<View> views = new List<View>();
        foreach (var model in models)
          views.Add(ViewGenerator.Create(model));
        return views;
    }

    public ApplicationBroker()
    {
        _assemblies = GetAllAssembliesSystem();

        TypeModels = FilterTypes(_assemblies, typeof(BaseModel));

        Models = CreateModels(TypeModels);

        Views = CreateViews(Models);
    }
}