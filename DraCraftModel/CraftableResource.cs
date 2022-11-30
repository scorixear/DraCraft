using Newtonsoft.Json;

namespace DraCraft.Model
{
    public class CraftableResource : Resource
    {
        [JsonIgnore]
        public Dictionary<(string, TierType), double> Resources { get; set; }
        public Dictionary<string, double> ResourceProxy
        {
            get
            {
                return SerializeResource(Resources);
            }
            set
            {
                Resources = DeserializeResource(value);
            }
        }
        public CraftableResource(string name, TierType tier, CategoryType category, Dictionary<(string, TierType), double> resources) : base(name, tier, category)
        {
            Resources = resources;
        }

        private Dictionary<string, double> SerializeResource(Dictionary<(string, TierType), double> resources)
        {
            Dictionary<string, double> returnDic = new();
            foreach (KeyValuePair<(string, TierType), double> pair in resources)
            {
                returnDic.Add($"({pair.Key.Item1}, {pair.Key.Item2})", pair.Value);
            }
            return returnDic;
        }

        private Dictionary<(string, TierType), double> DeserializeResource(Dictionary<string, double> resources)
        {
            Dictionary<(string, TierType), double> returnDic = new();
            foreach (KeyValuePair<string, double> pair in resources)
            {
                string[] keySplit = pair.Key.Split(",");
                returnDic.Add((keySplit[0][1..], Enum.Parse<TierType>(keySplit[1][1..^1])), pair.Value);
            }
            return returnDic;
        }

        public Dictionary<Resource, double> GetBaseResources()
        {
            Dictionary<Resource, double> returnDictionary = new Dictionary<Resource, double>();
            foreach (KeyValuePair<(string, TierType), double> ingredient in Resources)
            {
                Resource? resource = ResourceList.Current.Resources.Where(elem => elem.Name == ingredient.Key.Item1 && elem.Tier == ingredient.Key.Item2).FirstOrDefault();
                if (resource is null)
                {
                    resource = new Resource(ingredient.Key.Item1, ingredient.Key.Item2, CategoryType.Sonstiges);
                    ResourceList.Current.Resources.Add(resource);
                }
                if (resource is not CraftableResource)
                {
                    if (returnDictionary.ContainsKey(resource))
                    {
                        returnDictionary[resource] = returnDictionary[resource] + ingredient.Value;
                    }
                    else
                    {
                        returnDictionary.Add(resource, ingredient.Value);
                    }
                }
                else
                {
                    Dictionary<Resource, double>? subIngredients = (resource as CraftableResource)?.GetBaseResources();
                    foreach (KeyValuePair<Resource, double> pair in subIngredients ?? new Dictionary<Resource, double>())
                    {
                        if (returnDictionary.ContainsKey(pair.Key))
                        {
                            returnDictionary[pair.Key] = returnDictionary[pair.Key] + pair.Value * ingredient.Value;
                        }
                        else
                        {
                            returnDictionary.Add(pair.Key, pair.Value * ingredient.Value);
                        }
                    }
                }
            }
            return returnDictionary;
        }
        public TreeResource ResolveToTree(double amount)
        {
            TreeResource treeResource = new()
            {
                Name = Name,
                Tier = Tier,
                Category = Category,
                Amount = amount
            };


            foreach (KeyValuePair<(string, TierType), double> pair in Resources)
            {
                Resource? resource = ResourceList.Current.Resources.Where(elem => elem.Name == pair.Key.Item1 && elem.Tier == pair.Key.Item2).FirstOrDefault();
                if (resource is null)
                {
                    resource = new Resource(pair.Key.Item1, pair.Key.Item2, CategoryType.Sonstiges);
                    ResourceList.Current.Resources.Add(resource);
                }

                if (resource is CraftableResource craftableResource)
                {
                    treeResource.Resources.Add(craftableResource.ResolveToTree(amount * pair.Value));
                }
                else
                {
                    treeResource.Resources.Add(new TreeResource()
                    {
                        Name = resource.Name,
                        Tier = resource.Tier,
                        Category = resource.Category,
                        Amount = amount * pair.Value
                    });
                }
            }
            return treeResource;
        }
    }

    public class TreeResource
    {
        public string Name { get; set; }
        public TierType Tier { get; set; }
        public CategoryType Category { get; set; }

        public double Amount { get; set; }
        public List<TreeResource> Resources { get; set; } = new List<TreeResource>();
    }
}
