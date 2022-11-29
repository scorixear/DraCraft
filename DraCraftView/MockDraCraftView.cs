using DraCraftModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DraCraftView
{
    public class MockDraCraftView
    {
        public MockDraCraftView()
        {
            CraftItems = new ObservableCollection<CraftItem>(new CraftItem[]
               {
                new CraftItem(NWResources)
                {
                    Amount=0
                },
                new CraftItem(NWResources)
                {
                    SelectedItem = NWResources[0],
                    Amount = 100
                }
               });
        }

        public Visibility CraftableItemsVisible { get; set; } = Visibility.Visible;
        public ObservableCollection<Resource> NWResources { get; set; } = new ObservableCollection<Resource>(new Resource[]{
            new Resource("Leather", TierType.T1, CategoryType.Lederverarbeitung),
            new Resource("Leather", TierType.T4, CategoryType.Lederverarbeitung),
            new Resource("Leather", TierType.T5, CategoryType.Lederverarbeitung),
        });

        public Resource? SelectedResource { get; set; }
        public ObservableCollection<Recipe> RawResources { get; set; } = new ObservableCollection<Recipe>(new Recipe[] {
            new Recipe()
            {
                Name="Leather",
                Tier=TierType.T1,
                Category=CategoryType.Lederverarbeitung,
                Amount=10000,
            },
            new Recipe()
            {
                Name="Langer Item Name blablabla",
                Tier=TierType.T5,
                Category = CategoryType.Lederverarbeitung,
                Amount = 10000
            },
            new Recipe()
            {
                Name="Langer Item Name blablabla",
                Tier=TierType.T5,
                Category = CategoryType.Lederverarbeitung,
                Amount = 10000
            },
             new Recipe()
            {
                Name="Langer Item Name blablabla",
                Tier=TierType.T5,
                Category = CategoryType.Lederverarbeitung,
                Amount = 10000
            }
        });
        public uint ItemAmount { get; set; }
        public ObservableCollection<Recipe> CraftingSteps { get; set; } = new ObservableCollection<Recipe>(new Recipe[] {
            new Recipe()
            {
                Name="Leather",
                Tier=TierType.T1,
                Category=CategoryType.Lederverarbeitung,
                Amount=10,
                CraftingSteps = new ObservableCollection<Recipe>(new Recipe[]
                {
                    new Recipe()
                    {
                        Name="Iron",
                        Tier=TierType.T1,
                        Category=CategoryType.Schmelzen,
                        Amount=10,
                        CraftingSteps = new ObservableCollection<Recipe>(new Recipe[]
                        {
                            new Recipe()
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
            new Recipe()
            {
                Name="Hide",
                Tier=TierType.T1,
                Category = CategoryType.Lederverarbeitung,
                Amount=1
            }
        });

        public ObservableCollection<CraftItem> CraftItems { get; set; }

        private string _itemName = "TestItem";
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
            }
        }
        private TierType _itemTier = TierType.T1;
        public ObservableCollection<string> Tiers { get; set; } = new ObservableCollection<string>(Enum.GetValues<TierType>().Select(elem => Enum.GetName(elem) ?? ""));
        public string ItemTier
        {
            get => Enum.GetName(_itemTier) ?? "";

            set
            {
                _itemTier = Enum.Parse<TierType>(value);
            }
        }
        private CategoryType _itemCategory = CategoryType.Sonstiges;
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>(Enum.GetValues<CategoryType>().Select(elem => Enum.GetName(elem) ?? ""));
        public string ItemCategory
        {
            get => Enum.GetName(_itemCategory) ?? "";

            set
            {
                _itemCategory = Enum.Parse<CategoryType>(value);
            }
        }

    }
}
