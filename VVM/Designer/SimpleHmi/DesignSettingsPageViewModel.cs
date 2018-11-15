using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer.SimpleHmi
{
    class DesignSettingsPageViewModel : SettingsPageViewModel
    {
        public DesignSettingsPageViewModel() : base(new DesignPlcService())
        {

        }
    }
}
