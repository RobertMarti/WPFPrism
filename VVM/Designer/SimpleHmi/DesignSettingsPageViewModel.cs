using VVM.ViewModels;
using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer
{
    class DesignSettingsPageViewModel : SettingsPageViewModel
    {
        public DesignSettingsPageViewModel() : base(new DesignPlcService())
        {

        }
    }
}
