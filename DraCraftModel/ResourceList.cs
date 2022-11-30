using Newtonsoft.Json;

namespace DraCraft.Model
{
    public class ResourceList
    {
        public HashSet<Resource> Resources { get; set; } = new();
        public static ResourceList Current { get; set; } = new ResourceList();
        private ResourceList() { }
        public void Save()
        {
            if (File.Exists(Config.Current?.DatabasePath ?? throw new DatabaseNotSetException()))
            {
                File.Delete(Config.Current.DatabasePath);
            }
            if (Directory.Exists(Path.GetDirectoryName(Config.Current.DatabasePath)) is false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Config.Current.DatabasePath) ?? throw new InternalDirectoryException());
            }
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
            };
            File.WriteAllText(Config.Current.DatabasePath, JsonConvert.SerializeObject(this, settings));
        }
        public static void Load()
        {
            if (File.Exists(Config.Current?.DatabasePath ?? throw new DatabaseNotSetException()) is false)
            {
                ResourceList resourceList = new();
                resourceList.Resources.Add(new Resource("Tierhaut", TierType.T1, CategoryType.Lederverarbeitung));
                resourceList.Resources.Add(new Resource("Tierhaut", TierType.T4, CategoryType.Lederverarbeitung));
                resourceList.Resources.Add(new Resource("Tierhaut", TierType.T5, CategoryType.Lederverarbeitung));
                resourceList.Resources.Add(new CraftableResource("Raues Leder", TierType.T2, CategoryType.Lederverarbeitung, new Dictionary<(string, TierType), double>(){
                    {("Tierhaut", TierType.T1), 4 }
                }));
                resourceList.Save();
                Current = resourceList;
            }
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
            };
            Current = JsonConvert.DeserializeObject<ResourceList>(File.ReadAllText(Config.Current?.DatabasePath ?? throw new DatabaseNotSetException()), settings) ?? throw new InvalidDatabaseException();
        }

        private class InvalidDatabaseException : Exception { }
        private class DatabaseNotSetException : Exception { }
    }
}
