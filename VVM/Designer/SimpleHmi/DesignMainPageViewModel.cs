using VVM.Designer.SimpleHmi;
using VVM.ViewModels.SimpleHmi;

namespace VVM.Designer.SimpleHmi
{
    class DesignMainPageViewModel : MainPageViewModel 
    {
        public DesignMainPageViewModel() : base(new DesignPlcService())
        {

        }
    }
}
