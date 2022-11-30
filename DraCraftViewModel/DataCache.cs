namespace DraCraft.ViewModel
{
    public class DataCache
    {
        private static DataCache singletonInstance;
        private DataCache()
        {

        }

        public static DataCache Instance
        {
            get
            {
                return singletonInstance ??= new DataCache();
            }
        }
    }
}
