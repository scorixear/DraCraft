using DraCraft.Model;
using System.Collections.ObjectModel;

namespace DraCraft.ViewModel
{
    public class CraftingStepControlModel : ViewModel
    {
        public delegate void DispatcherEventHandler(Action action);
        public event DispatcherEventHandler DispatcherEvent;
        internal void OnDispatcherEvent(Action action)
        {
            DispatcherEvent?.Invoke(action);
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
                OnPropertyChanged(nameof(ItemAmount));
                UpdateCraftingSteps();
                onPropertyChangedAction.Invoke(typeof(RawResourceControlModel), "RawResources");
            }
        }

        private ObservableCollection<CraftingItem> _craftingSteps;
        public ObservableCollection<CraftingItem> CraftingSteps
        {
            get
            {
                return _craftingSteps;
            }
            set
            {
                _craftingSteps = value;
                OnPropertyChanged(nameof(CraftingSteps));
            }
        }
        private readonly Action<Type, string> onPropertyChangedAction;
        private readonly Func<Resource?> getSelectedResourceFunc;

        internal CraftingStepControlModel(Action<Type, string> onPropertyChangedAction, Func<Resource?> getSelectedResourceFunc)
        {
            this.onPropertyChangedAction = onPropertyChangedAction;
            this.getSelectedResourceFunc = getSelectedResourceFunc;
        }


        public void SetItemAmount(uint amount)
        {
            ItemAmount = amount;
        }

        internal void UpdateCraftingSteps()
        {
            if (getSelectedResourceFunc.Invoke() is not null)
            {
                if (getSelectedResourceFunc.Invoke() is CraftableResource craftResource)
                {
                    CraftingSteps = new ObservableCollection<CraftingItem>(CraftingItem.TransformFromResource(this, craftResource.ResolveToTree(ItemAmount), () =>
                    {
                        onPropertyChangedAction.Invoke(typeof(RawResourceControlModel), "RawResources");
                    }).CraftingSteps);
                }
                else
                {
                    CraftingSteps = new ObservableCollection<CraftingItem>(new CraftingItem[] { new CraftingItem(this, getSelectedResourceFunc.Invoke(), ItemAmount, () =>
                        {
                            onPropertyChangedAction.Invoke(typeof(RawResourceControlModel), "RawResources");
                        })
                    });
                }
            }
            else
            {
                CraftingSteps = new ObservableCollection<CraftingItem>();
            }
        }
    }


}
