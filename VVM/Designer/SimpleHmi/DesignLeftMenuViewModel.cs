using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using SimpleHmi.Infrastructure;

namespace VVM.ViewModels.SimpleHmi
{
    class LeftMenuViewModel
    {
        public ICommand NavigateToMainPageCommand { get; private set; }

        public ICommand NavigateToSettingsPageCommand { get; private set; }

        private readonly IRegionManager _regionManager;

        public LeftMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateToMainPageCommand = new DelegateCommand(() => NavigateTo("MainPage"));
            NavigateToSettingsPageCommand = new DelegateCommand(() => NavigateTo("SettingsPage"));
        }

        private void NavigateTo(string url)
        {
            _regionManager.RequestNavigate(Regions.ContentRegion, url);
        }
    }
}
