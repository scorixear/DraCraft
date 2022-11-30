namespace DraCraft.ViewModel
{
    public class MainWindowModel : ViewModel
    {


        private readonly ResourceControlModel resourceVM;
        private readonly CraftingStepControlModel craftingStepVM;
        private readonly RawResourceControlModel rawResourcesVM;
        private readonly CreatorControlModel creatorVM;
        public MainWindowModel()
        {
            resourceVM = new ResourceControlModel(HandlePropertyChangedRequest);

            craftingStepVM = new CraftingStepControlModel(HandlePropertyChangedRequest, () => ResourceVM.SelectedResource);
            rawResourcesVM = new RawResourceControlModel(() => craftingStepVM.CraftingSteps);
            creatorVM = new CreatorControlModel(HandlePropertyChangedRequest, () => ResourceVM.NWResources);
            resourceVM.CraftingStepUpdate += delegate
            {
                craftingStepVM.UpdateCraftingSteps();
            };
        }
        private void HandlePropertyChangedRequest(Type type, string propertyName)
        {
            if (type == typeof(ResourceControlModel))
            {
                ResourceVM.OnPropertyChanged(propertyName);
            }
            else if (type == typeof(CraftingStepControlModel))
            {
                CraftingStepVM.OnPropertyChanged(propertyName);
            }
            else if (type == typeof(RawResourceControlModel))
            {
                RawResourcesVM.OnPropertyChanged(propertyName);
            }
            else if (type == typeof(CreatorControlModel))
            {
                CreatorVM.OnPropertyChanged(propertyName);
            }
            else
            {
                OnPropertyChanged(propertyName);
            }
        }

        public ResourceControlModel ResourceVM { get { return resourceVM; } }
        public CraftingStepControlModel CraftingStepVM { get { return craftingStepVM; } }
        public RawResourceControlModel RawResourcesVM { get { return rawResourcesVM; } }
        public CreatorControlModel CreatorVM { get { return creatorVM; } }
    }
}