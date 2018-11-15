using Prism.Mvvm;
using Prism.Regions;
using SimpleHmi.Infrastructure;
using VVM.Views.SimpleHmi;

namespace VVM.ViewModels.SimpleHmi
{
    class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion(Regions.ContentRegion, typeof(MainPage));
            _regionManager.RegisterViewWithRegion(Regions.StatusBarRegion, typeof(HmiStatusBar));
            _regionManager.RegisterViewWithRegion(Regions.LeftMenuRegion, typeof(LeftMenu));
        }
    }
}
