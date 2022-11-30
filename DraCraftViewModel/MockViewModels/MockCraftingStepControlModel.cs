using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class MockCraftingStepControlModel : CraftingStepControlModel
    {
        public MockCraftingStepControlModel() : base((a, b) => { }, () => { return null; })
        {
            CraftingSteps = new ObservableCollection<CraftingItem>(new CraftingItem[] {
                new CraftingItem(this)
                {
                    Name="Leather",
                    Tier=TierType.T1,
                    Category=CategoryType.Lederverarbeitung,
                    Amount=10,
                    CraftingSteps = new ObservableCollection<CraftingItem>(new CraftingItem[]
                    {
                        new CraftingItem(this)
                        {
                            Name="Iron",
                            Tier=TierType.T1,
                            Category=CategoryType.Schmelzen,
                            Amount=10,
                            CraftingSteps = new ObservableCollection<CraftingItem>(new CraftingItem[]
                            {
                                new CraftingItem(this)
                                {
                                    Name="IronOre",
                                    Tier=TierType.T1,
                                    Category = CategoryType.Schmelzen,
                                    Amount=40
                                }
                            })
                        }
                    })
                },
                new CraftingItem(this)
                {
                    Name="Hide",
                    Tier=TierType.T1,
                    Category = CategoryType.Lederverarbeitung,
                    Amount=1
                }
            });
        }
    }
}
