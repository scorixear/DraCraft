using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class RawResourceControlModel : ViewModel
    {
        public ObservableCollection<RawResourceItem> RawResources
        {
            get
            {
                List<RawResourceItem> list = new();
                if (getCraftingSteps.Invoke() is not null)
                {
                    foreach (CraftingItem recipe in getCraftingSteps.Invoke())
                    {
                        if (recipe.IsDone)
                        {
                            continue;
                        }
                        List<RawResourceItem> amounts = recipe.GetRawResources();
                        foreach (RawResourceItem element in amounts)
                        {
                            RawResourceItem? foundElement = list.Find(e => e.Name == element.Name && e.Tier == element.Tier);
                            if (foundElement is not null)
                            {
                                foundElement.Amount += element.Amount;
                            }
                            else
                            {
                                list.Add(element);
                            }
                        }
                    }
                }
                return new ObservableCollection<RawResourceItem>(list.OrderBy(elem => elem.Category).ThenBy(elem => elem.Name).ThenBy(elem => elem.Tier));
            }
        }

        private readonly Func<ObservableCollection<CraftingItem>> getCraftingSteps;
        internal RawResourceControlModel(Func<ObservableCollection<CraftingItem>> getCraftingSteps)
        {
            this.getCraftingSteps = getCraftingSteps;
        }
    }
}
