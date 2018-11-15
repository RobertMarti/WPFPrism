using System.Windows;
using Autofac;
using Prism.Autofac;
using SimpleHmi.PlcService;
using VVM.Views;
using VVM.Views.SimpleHmi;

namespace VVM
{
    class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            MessageBox.Show("SimpleHmi");
            return Container.Resolve<MainWindow>();

            MessageBox.Show("Menu");
            return Container.Resolve<Menu>();
        }

        protected override void InitializeShell()
        {
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);
            //builder.RegisterType<S7PlcService>().As<IPlcService>().SingleInstance();
            builder.RegisterType<MockPlcService>().As<IPlcService>().SingleInstance();
            builder.RegisterTypeForNavigation<MainPage>("MainPage");
            builder.RegisterTypeForNavigation<LeftMenu>("LeftMenu");
            builder.RegisterTypeForNavigation<HmiStatusBar>("HmiStatusBar");
            builder.RegisterTypeForNavigation<SettingsPage>("SettingsPage");
        }
    }
}
