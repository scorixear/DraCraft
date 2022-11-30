using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class MockResourceControlModel : ResourceControlModel
    {
        public MockResourceControlModel() : base((a, b) => { })
        {
            NWResources = new ObservableCollection<Resource>(new Resource[]{
            new Resource("Leather", TierType.T1, CategoryType.Lederverarbeitung),
            new Resource("Leather", TierType.T4, CategoryType.Lederverarbeitung),
            new Resource("Leather", TierType.T5, CategoryType.Lederverarbeitung) });
        }
    }
}
