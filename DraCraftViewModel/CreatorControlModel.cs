using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class CreatorControlModel : ViewModel
    {
        private string? _itemName = string.Empty;
        public string? ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }

        public ObservableCollection<string> Tiers { get; set; } = new ObservableCollection<string>(Enum.GetValues<TierType>().Select(elem => Enum.GetName(elem) ?? "").Order());

        private TierType? _itemTier = null;
        public string? ItemTier
        {
            get
            {
                if (_itemTier == null)
                {
                    return null;
                }
                return Enum.GetName(_itemTier.Value) ?? null;
            }

            set
            {
                if (value == null)
                {
                    _itemTier = null;
                }
                else
                {
                    _itemTier = Enum.Parse<TierType>(value);
                }
                OnPropertyChanged(nameof(ItemTier));
            }
        }

        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>(Enum.GetValues<CategoryType>().Select(elem => Enum.GetName(elem) ?? "").Order());
        private CategoryType? _itemCategory = null;
        public string? ItemCategory
        {
            get
            {
                if (_itemCategory == null)
                {
                    return null;
                }
                return Enum.GetName(_itemCategory.Value) ?? null;
            }

            set
            {
                if (value == null)
                {
                    _itemCategory = null;
                }
                else
                {
                    _itemCategory = Enum.Parse<CategoryType>(value);
                }
                OnPropertyChanged(nameof(ItemCategory));
            }
        }

        private bool _craftableItemsVisible = false;
        public bool CraftableItemsVisible
        {
            get
            {
                return _craftableItemsVisible;
            }
            set
            {
                _craftableItemsVisible = value;
                OnPropertyChanged(nameof(CraftableItemsVisible));
            }
        }

        private List<ComponentItem> _craftItems = new();
        public ObservableCollection<ComponentItem> CraftItems
        {
            get
            {
                return new ObservableCollection<ComponentItem>(_craftItems);
            }
            set
            {
                _craftItems = new List<ComponentItem>(value);
                OnPropertyChanged(nameof(CraftItems));
            }
        }

        private readonly Action<Type, string> propertyChangedRequestAction;
        private readonly Func<ObservableCollection<Resource>> getNWResourcesFunc;
        internal CreatorControlModel(Action<Type, string> propertyChangedRequestAction, Func<ObservableCollection<Resource>> getNWResourcesFunc)
        {
            this.propertyChangedRequestAction = propertyChangedRequestAction;
            this.getNWResourcesFunc = getNWResourcesFunc;
        }


        public void CraftableToggle(bool value)
        {
            CraftableItemsVisible = value;
        }

        public void CreateItem()
        {
            Resource resource;
            if (_itemCategory == null || _itemCategory == null || string.IsNullOrEmpty(ItemName?.Trim()))
            {
                return;
            }
            if (CraftableItemsVisible == true)
            {
                Dictionary<(string, TierType), double> craftingResources = new();
                foreach (ComponentItem craftItem in CraftItems)
                {
                    if (craftItem.SelectedItem != null)
                    {
                        craftingResources.Add((craftItem.SelectedItem.Name, craftItem.SelectedItem.Tier), craftItem?.Amount ?? 1);
                    }
                }
                resource = new CraftableResource(ItemName, _itemTier ?? TierType.T1, _itemCategory ?? CategoryType.Sonstiges, craftingResources);
            }
            else
            {
                resource = new Resource(ItemName, _itemTier ?? TierType.T1, _itemCategory ?? CategoryType.Sonstiges);
            }
            ResourceList.Current.Resources.Add(resource);
            ItemName = null;
            ItemTier = null;
            ItemCategory = null;
            CraftItems = new();

            propertyChangedRequestAction(typeof(ResourceControlModel), "NWResources");
        }

        public void AddCraftItem()
        {
            List<ComponentItem> items = CraftItems.ToList();
            items.Insert(0, new ComponentItem(getNWResourcesFunc.Invoke()));

            CraftItems = new ObservableCollection<ComponentItem>(items);
        }

        public void RemoveComponent(object? dataContext)
        {
            ComponentItem? selectedItem = dataContext as ComponentItem;
            if (selectedItem is not null)
            {
                List<ComponentItem> items = CraftItems.ToList();
                items.Remove(selectedItem);
                CraftItems = new ObservableCollection<ComponentItem>(items);
            }
        }

    }
}
