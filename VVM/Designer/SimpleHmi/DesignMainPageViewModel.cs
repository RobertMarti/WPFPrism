using VVM.ViewModels;
using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer
{
    class DesignMainPageViewModel : MainPageViewModel 
    {
        public DesignMainPageViewModel() : base(new DesignPlcService())
        {

        }
    }
}
