namespace DraCraft.Model
{
    public class Resource : IEquatable<Resource>
    {
        public string Name { get; set; }
        public TierType Tier { get; set; }
        public CategoryType Category { get; set; }

        public Resource(string name, TierType tier, CategoryType category)
        {
            Name = name;
            Tier = tier;
            Category = category;
        }

        public bool Equals(Resource? other)
        {
            return Name == other?.Name && Tier == other?.Tier;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Resource other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Tier);
        }
    }
}
