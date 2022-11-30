using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class MockRawResourceControlModel : RawResourceControlModel
    {
        public MockRawResourceControlModel() : base(() => null)
        {
            RawResources = new ObservableCollection<RawResourceItem>(new RawResourceItem[] {
                new RawResourceItem()
                {
                    Name="Leather",
                    Tier=TierType.T1,
                    Category=CategoryType.Lederverarbeitung,
                    Amount=10000,
                },
                new RawResourceItem()
                {
                    Name="Langer Item Name blablabla",
                    Tier=TierType.T5,
                    Category = CategoryType.Lederverarbeitung,
                    Amount = 10000
                },
                new RawResourceItem()
                {
                    Name="Langer Item Name blablabla",
                    Tier=TierType.T5,
                    Category = CategoryType.Lederverarbeitung,
                    Amount = 10000
                },
                new RawResourceItem()
                {
                    Name="Langer Item Name blablabla",
                    Tier=TierType.T5,
                    Category = CategoryType.Lederverarbeitung,
                    Amount = 10000
                }
            });
        }
        public new ObservableCollection<RawResourceItem> RawResources
        {
            get;
            set;
        }
    }
}
