using System;
using Prism.Mvvm;
using SimpleHmi.PlcService;

namespace VVM.ViewModels.SimpleHmi
{
    class HmiStatusBarViewModel : BindableBase
    {
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }
        private ConnectionStates _connectionState;

        public TimeSpan ScanTime
        {
            get { return _scanTime; }
            set { SetProperty(ref _scanTime, value); }
        }
        private TimeSpan _scanTime;

        private readonly IPlcService _plcService;

        public HmiStatusBarViewModel(IPlcService plcService)
        {
            _plcService = plcService;
            _plcService.ValuesRefreshed += OnPlcServiceValuesRefreshed;
            OnPlcServiceValuesRefreshed(null, EventArgs.Empty);
        }

        private void OnPlcServiceValuesRefreshed(object sender, EventArgs e)
        {
            ConnectionState = _plcService.ConnectionState;
            ScanTime = _plcService.ScanTime;
        }
    }
}
