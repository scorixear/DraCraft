using DraCraft.Model;

namespace DraCraft.ViewModel
{
    public class RawResourceItem
    {
        public string Name { get; set; }
        public TierType Tier { get; set; }
        public CategoryType Category { get; set; }
        public double Amount { get; set; }
    }
}
