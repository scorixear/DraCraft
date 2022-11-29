using Newtonsoft.Json;

namespace DraCraftModel
{
    public class Config
    {
        public static readonly string AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public static readonly string AppFolder = Path.GetDirectoryName(AppPath) ?? "";
        public static readonly string ConfigPath = Path.Combine(AppFolder, "config.json");
        public static Config? Current { get; private set; }
        private Config(InternalConfig internalConfig) {
            this.internalConfig = internalConfig;
        }
        private InternalConfig internalConfig;

        public string DatabasePath {
            get
            {
                return internalConfig.DataBasePath;
            }
            set
            {
                internalConfig.DataBasePath = value;
                Save();
            }
        }
        private class InternalConfig
        {
            public string DataBasePath = Path.Combine(AppFolder, "database.json");
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(internalConfig);
            if (File.Exists(ConfigPath))
            {
                File.Delete(ConfigPath);
            }
            if (Directory.Exists(Path.GetDirectoryName(ConfigPath)) is false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath) ?? throw new InternalDirectoryException());
            }
            File.WriteAllText(json, ConfigPath);
        }
        private static InternalConfig Load()
        {
            if (File.Exists(ConfigPath))
            {
                return JsonConvert.DeserializeObject<InternalConfig>(File.ReadAllText(ConfigPath)) ?? throw new InvalidConfigException();
            } 
            else
            {
                InternalConfig config = new InternalConfig();
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                return config;
            }
        }

        public static void Init()
        {
            Current = new Config(Load());
        }


        private class InvalidConfigException : Exception { }
        private class ConfigNotFoundException : Exception { }
    }
}
