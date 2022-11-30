using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class MockCreatorControlModel : CreatorControlModel
    {
        public MockCreatorControlModel() : base((a, b) => { }, () => null)
        {
            ItemName = "TestItem";
            ItemTier = "T1";
            ItemCategory = "Sonstiges";

            CraftableItemsVisible = true;
            Resource tmpResource = new("Test Resource", TierType.T5, CategoryType.Waffen);
            CraftItems = new ObservableCollection<ComponentItem>(new ComponentItem[]
            {
                new ComponentItem(new ObservableCollection<Resource>(new Resource[]{tmpResource}))
                {
                    SelectedItem = tmpResource,
                    Amount = 25.666
                }
            });
        }
    }
}
