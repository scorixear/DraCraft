using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class ComponentItem
    {
        public ComponentItem(ObservableCollection<Resource> resources)
        {
            Resources = resources;
        }
        public ObservableCollection<Resource> Resources { get; set; }
        public Resource? SelectedItem { get; set; }
        public double? Amount { get; set; }
    }
}
