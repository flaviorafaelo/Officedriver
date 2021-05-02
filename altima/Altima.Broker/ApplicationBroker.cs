using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using Altima.Broker.System;
using Altima.Broker.Core;
using Altima.Broker.Business;
using System.Reflection;
using Altima.Broker.Metadata.Generator;
using System.IO;
using Newtonsoft.Json;
using broker = Altima.Broker.System.Routes;
using Altima.Broker.System.Routes;
using Altima.Broker.Extensions;

public class ApplicationBroker : IApplicationBroker
{
    private readonly IList<Assembly> _assemblies;
    public IList<Type> TypeModels { get; private set; }
    public IList<Model> Models { get; private set; }
    public IList<View> Views { get; private set; }
    public IList<broker.Module> Modules { get; set; }

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
                if (model.Name.Contains("Cooperado") || model.Name.Contains("ClienteX") || model.Name.Contains("User"))
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

    private IList<broker.Module> LoadModules()
    {
        var referencedPaths = Workaround.GetFiles("*.routes");
        var modules = new List<broker.Module>();

        foreach (var reference in referencedPaths)
        {
            string json = File.ReadAllText(reference);
            var fileModules = JsonConvert.DeserializeObject<List<broker.Module>>(json);
            foreach (var module in fileModules)
            {
                foreach (var route in module.Routes)
                {
                    route.Id = $"{module.Id}.{route.Id}";
                    route.Url = route.Display.FormatToUrl();

                    var idAction = 1;

                    if (route.Actions != null)
                    {
                        foreach (var action in route.Actions)
                        {
                            action.Id = $"{route.Id}.{action.Id}";
                            action.Url = $"{route.Url}/{action.Display.FormatToUrl()}" ;
                        }
                           
                    }

                    var crudAction = route.Actions?.Where(r => r.Type == ActionType.CRUD).FirstOrDefault();
                    if (crudAction != null)
                    {
                        //Create
                        route.Actions.Add(new broker.Action($"{crudAction.Id}.{idAction++}", "Criar", ActionType.Create, crudAction.Target, null));

                        //Update
                        route.Actions.Add(new broker.Action($"{crudAction.Id}.{idAction++}", "Alterar", ActionType.Update, crudAction.Target, new Service(route.Service.Name, new List<Param>() { new Param("id", "{data.id}") })));

                        //Excluir
                        route.Actions.Add(new broker.Action($"{crudAction.Id}.{idAction++}", "Excluir", ActionType.Create, crudAction.Target, new Service(route.Service.Name, new List<Param>() { new Param("id", "{data.id}") })));

                        route.Actions.Remove(crudAction);
                    }
                }
            }
            modules.AddRange(fileModules);
        }

        return modules;
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

        Modules = LoadModules();
    }
}