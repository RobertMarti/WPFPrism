using VVM.ViewModels;
using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer
{
    class DesignHmiStatusBarViewModel : HmiStatusBarViewModel
    {
        public DesignHmiStatusBarViewModel() : base(new DesignPlcService())
        {
                
        }
    }
}
