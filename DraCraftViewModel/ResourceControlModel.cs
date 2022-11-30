using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class ResourceControlModel : ViewModel
    {
        public ObservableCollection<Resource> NWResources
        {
            get
            {
                return new ObservableCollection<Resource>(ResourceList.Current.Resources.OrderBy(element => element.Name).ThenBy(element => element.Tier).ThenBy(element => element.Category));
            }
            set
            {
                ResourceList.Current.Resources = new HashSet<Resource>(value);
                OnPropertyChanged(nameof(NWResources));
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
                OnPropertyChanged(nameof(SelectedResource));
                CraftingStepUpdate?.Invoke(this, new EventArgs());
                onPropertyChangedAction.Invoke(typeof(RawResourceControlModel), "RawResources");
            }
        }

        internal event EventHandler? CraftingStepUpdate;
        private readonly Action<Type, string> onPropertyChangedAction;
        public ResourceControlModel(Action<Type, string> onPropertyChangedAction)
        {
            this.onPropertyChangedAction = onPropertyChangedAction;
        }

        public void RemoveResource(object? dataContext)
        {
            Resource? selectedItem = dataContext as Resource;
            if (selectedItem is not null)
            {
                ResourceList.Current.Resources.Remove(selectedItem);
                OnPropertyChanged(nameof(NWResources));
            }
        }
    }
}
