using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer.SimpleHmi
{
    class DesignHmiStatusBarViewModel : HmiStatusBarViewModel
    {
        public DesignHmiStatusBarViewModel() : base(new DesignPlcService())
        {
                
        }
    }
}
