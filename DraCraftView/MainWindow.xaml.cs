using DraCraftModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DraCraftView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Resource> NWResources
        {
            get
            {
                return new ObservableCollection<Resource>(ResourceList.Current.Resources.OrderBy(element => element.Category).ThenBy(element => element.Name).ThenBy(element => element.Tier));
            }
            set
            {
                ResourceList.Current.Resources = new HashSet<Resource>(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NWResources)));
            }
        }

        private Resource? _selectedResource;
        public Resource? SelectedResource
        {
            get
            {
                return _selectedResource;
            }
            set
            {
                _selectedResource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedResource)));
                UpdateCraftingSteps();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawResources)));
            }
        }

        private ObservableCollection<Recipe> _craftingSteps;
        private void UpdateCraftingSteps()
        {
            if (SelectedResource is not null)
            {
                if (SelectedResource is CraftableResource craftResource)
                {
                    CraftingSteps = new ObservableCollection<Recipe>(Recipe.TransformFromResource(craftResource.ResolveToTree(ItemAmount), () =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawResources)));
                    }).CraftingSteps);
                }
                else
                {
                    CraftingSteps = new ObservableCollection<Recipe>(new Recipe[] { new Recipe(SelectedResource, ItemAmount, () =>
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawResources)));
                        })
                    });
                }
            }
            else
            {
                CraftingSteps = new ObservableCollection<Recipe>();
            }
        }

        public ObservableCollection<Recipe> CraftingSteps
        {
            get
            {
                return _craftingSteps;
            }
            set
            {
                _craftingSteps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CraftingSteps)));
            }
        }

        public ObservableCollection<Recipe> RawResources
        {
            get
            {
                List<Recipe> list = new();
                if (CraftingSteps is not null)
                {
                    foreach (Recipe recipe in CraftingSteps)
                    {
                        if (recipe.IsDone)
                        {
                            continue;
                        }
                        List<Recipe> amounts = recipe.GetRawResources();
                        foreach (Recipe element in amounts)
                        {
                            Recipe? foundElement = list.Find(e => e.Name == element.Name && e.Tier == element.Tier);
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
                return new ObservableCollection<Recipe>(list.OrderBy(elem => elem.Category).ThenBy(elem => elem.Name).ThenBy(elem => elem.Tier));
            }
        }

        private uint _itemAmount = 1;
        public uint ItemAmount
        {
            get
            {
                return _itemAmount;
            }
            set
            {
                _itemAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemAmount)));
                UpdateCraftingSteps();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawResources)));
            }
        }

        private Visibility _craftableItemsVisible = Visibility.Hidden;
        public Visibility CraftableItemsVisible
        {
            get
            {
                return _craftableItemsVisible;
            }
            set
            {
                _craftableItemsVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CraftableItemsVisible)));
            }
        }

        private List<CraftItem> _craftItems = new();
        public ObservableCollection<CraftItem> CraftItems
        {
            get
            {
                return new ObservableCollection<CraftItem>(_craftItems);
            }
            set
            {
                _craftItems = new List<CraftItem>(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CraftItems)));
            }
        }


        // Create Item Bindings
        private string? _itemName = string.Empty;
        public string? ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemName)));
            }
        }
        private TierType? _itemTier = null;
        public ObservableCollection<string> Tiers { get; set; } = new ObservableCollection<string>(Enum.GetValues<TierType>().Select(elem => Enum.GetName(elem) ?? "").Order());
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemTier)));
            }
        }
        private CategoryType? _itemCategory = null;
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>(Enum.GetValues<CategoryType>().Select(elem => Enum.GetName(elem) ?? "").Order());
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemCategory)));
            }
        }


        public MainWindow()
        {
            DataContext = this;
            Config.Init();
            ResourceList.Load();
            InitializeComponent();
        }

        private void AmountTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (uint.TryParse(((TextBox)sender).Text, out uint result))
            {
                ItemAmount = result;
            }
        }

        private void AddCraftItem_Click(object sender, RoutedEventArgs e)
        {
            List<CraftItem> items = CraftItems.ToList();
            items.Insert(0, new CraftItem(NWResources));

            CraftItems = new ObservableCollection<CraftItem>(items);
        }

        private void CreateItem_Click(object sender, RoutedEventArgs e)
        {
            Resource resource;
            if (_itemCategory == null || _itemCategory == null || string.IsNullOrEmpty(ItemName?.Trim()))
            {
                return;
            }
            if (CraftableItemsVisible == Visibility.Visible)
            {
                Dictionary<(string, TierType), uint> craftingResources = new();
                foreach (CraftItem craftItem in CraftItems)
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NWResources)));
        }

        private void CraftableCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked ?? false)
            {
                CraftableItemsVisible = Visibility.Visible;
            }
            else
            {
                CraftableItemsVisible = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ResourceList.Current.Save();
        }

        private void RemoveCraftItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContentPresenter? contentPresenter = ((Grid)button.Parent).TemplatedParent as ContentPresenter;
            CraftItem? selectedItem = contentPresenter?.DataContext as CraftItem;
            if (selectedItem is not null)
            {
                List<CraftItem> items = CraftItems.ToList();
                items.Remove(selectedItem);
                CraftItems = new ObservableCollection<CraftItem>(items);
            }
        }

        private void RemoveResource_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContentPresenter? contentPresenter = ((Grid)button.Parent).TemplatedParent as ContentPresenter;
            Resource? selectedItem = contentPresenter?.DataContext as Resource;
            if (selectedItem is not null)
            {
                ResourceList.Current.Resources.Remove(selectedItem);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NWResources)));
            }
        }
    }
    public class Recipe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        internal Action? _removeFromResources;
        public Recipe(Action? removeFromResources = null)
        {
            _removeFromResources = removeFromResources;
        }
        public Recipe(Resource resource, uint amount, Action? removeFromResources = null) : this(removeFromResources)
        {
            Name = resource.Name;
            Tier = resource.Tier;
            Category = resource.Category;
            Amount = amount;
            InitAmount = amount;
        }
        public string Name { get; set; }
        public TierType Tier { get; set; }
        public CategoryType Category { get; set; }
        public uint Amount { get; set; }
        internal uint InitAmount { get; set; }
        public ObservableCollection<Recipe> CraftingSteps { get; set; }
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
                    foreach (Recipe craftingStep in CraftingSteps)
                    {
                        craftingStep.IsDone = IsDone;
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _removeFromResources?.Invoke();
                });
            }
        }

        public List<Recipe> GetRawResources()
        {
            List<Recipe> resources = new();
            if (CraftingSteps is not null)
            {
                foreach (Recipe recipe in CraftingSteps)
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
                resources.Add(new Recipe()
                {
                    Name = Name,
                    Tier = Tier,
                    Category = Category,
                    Amount = Amount,
                });
            }
            return resources;
        }

        public static Recipe TransformFromResource(TreeResource resource, Action removeFromResources)
        {
            Recipe returnValue = new()
            {
                Name = resource.Name,
                Tier = resource.Tier,
                Category = resource.Category,
                Amount = resource.Amount,
                InitAmount = resource.Amount
            };
            returnValue._removeFromResources = removeFromResources;

            if (resource.Resources.Count > 0)
            {
                List<Recipe> result = new();
                foreach (TreeResource res in resource.Resources)
                {
                    result.Add(TransformFromResource(res, removeFromResources));
                }
                returnValue.CraftingSteps = new ObservableCollection<Recipe>(result);
            }
            return returnValue;
        }
    }

    public class CraftItem
    {
        public CraftItem(ObservableCollection<Resource> resources)
        {
            Resources = resources;
        }
        public ObservableCollection<Resource> Resources { get; set; }
        public Resource? SelectedItem { get; set; }
        public uint? Amount { get; set; }
    }
}



