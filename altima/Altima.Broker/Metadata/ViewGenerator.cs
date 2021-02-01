using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Altima.Broker.System;
using Altima.Broker.System.Serializable;

namespace Altima.Broker.Metadata.Generator
{
    public static class ViewGenerator
    {

        public static View Create(Model model)
        {

            var referencedPaths = Workaround.GetFiles(model.Name+".view");
            if (referencedPaths.Count == 1)
            {
                ViewFile viewFile = JsonConvert.DeserializeObject<ViewFile>(File.ReadAllText(referencedPaths[0]));

                IList<Page> pages = new List<Page>();
                foreach (var pageFile in viewFile.Pages)
                {

                    IList<Group> groups = new List<Group>();
                    foreach (var groupFile in pageFile.Groups)
                    {
                        IList<Item> items = new List<Item>();
                        foreach (var itemFile in groupFile.Items)
                        {
                            foreach (var property in model.Properties)//melhorar isso, nada performativo. Colocar uma pesquisa binaaria
                            {
                                if (property.Name.Equals(itemFile.Property, StringComparison.OrdinalIgnoreCase))
                                {
                                    Item item = new Item(property, itemFile.Label, itemFile.VisibilityRule, itemFile.EnablingRule, itemFile.Tooltip);
                                    items.Add(item);
                                }
                            }
                        }
                        Group group = new Group(groupFile.Description, items);
                        groups.Add(group);
                    }

                    Page page = new Page(pageFile.Description, groups);
                    pages.Add(page);
                }

                //verifica se todas as propriedades foram adicionadas?

                return new View(viewFile.Description, viewFile.Version, model, pages);
            }
            else
            {
                IList<Item> items = new List<Item>();
                foreach (var property in model.Properties)
                {
                    Item item = new Item(property, property.Name, null, null, null);
                    items.Add(item);
                }

                Group group = new Group(null, items);

                IList<Group> groups = new List<Group>();
                groups.Add(group);

                Page page = new Page(null, groups);
                IList<Page> pages = new List<Page>();
                pages.Add(page);

                return new View(model.Name, 1, model, pages);
            }
        }
    }
}

// criar conceito de IDataTypeLooup ou criar na view
