using DraCraft.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DraCraft.ViewModel
{
    public class CraftingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private CraftingStepControlModel ViewModel;
        internal Action? _removeFromResources;
        public CraftingItem(CraftingStepControlModel model, Action? removeFromResources = null)
        {
            _removeFromResources = removeFromResources;
            ViewModel = model;
        }
        public CraftingItem(CraftingStepControlModel model, Resource resource, double amount, Action? removeFromResources = null) : this(model, removeFromResources)
        {
            Name = resource.Name;
            Tier = resource.Tier;
            Category = resource.Category;
            Amount = amount;
        }
        public string Name { get; set; }
        public TierType Tier { get; set; }
        public CategoryType Category { get; set; }
        public double Amount { get; set; }
        public ObservableCollection<CraftingItem> CraftingSteps { get; set; }
        private bool _isDone;
        public bool IsDone
        {
            get
            {
                return _isDone;
            }
            set
            {
                _isDone = value;
                Expanded = !value;
                OnPropertyChanged(nameof(IsDone));

            }
        }
        private bool _expanded = true;
        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;
                OnPropertyChanged(nameof(Expanded));
            }
        }

        internal void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            if (name == nameof(IsDone))
            {
                if (CraftingSteps is not null)
                {
                    foreach (CraftingItem craftingStep in CraftingSteps)
                    {
                        craftingStep.IsDone = IsDone;
                    }
                }
                ViewModel.OnDispatcherEvent(() =>
                {
                    _removeFromResources?.Invoke();
                });
            }
        }

        public List<RawResourceItem> GetRawResources()
        {
            List<RawResourceItem> resources = new();
            if (CraftingSteps is not null)
            {
                foreach (CraftingItem recipe in CraftingSteps)
                {
                    if (recipe.IsDone)
                    {
                        continue;
                    }
                    resources.AddRange(recipe.GetRawResources());
                }
            }
            else
            {
                resources.Add(new RawResourceItem()
                {
                    Name = Name,
                    Tier = Tier,
                    Category = Category,
                    Amount = Amount,
                });
            }
            return resources;
        }

        public static CraftingItem TransformFromResource(CraftingStepControlModel model, TreeResource resource, Action removeFromResources)
        {
            CraftingItem returnValue = new(model)
            {
                Name = resource.Name,
                Tier = resource.Tier,
                Category = resource.Category,
                Amount = resource.Amount,
                _removeFromResources = removeFromResources
            };

            if (resource.Resources.Count > 0)
            {
                List<CraftingItem> result = new();
                foreach (TreeResource res in resource.Resources)
                {
                    result.Add(TransformFromResource(model, res, removeFromResources));
                }
                returnValue.CraftingSteps = new ObservableCollection<CraftingItem>(result);
            }
            return returnValue;
        }
    }
}
