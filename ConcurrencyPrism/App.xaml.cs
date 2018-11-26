using System;
using System.Windows;
using ConcurrencyPrism.Views;
using Prism.Unity;
using Prism.Ioc;

namespace ConcurrencyPrism
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : PrismApplication
  {
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      //throw new NotImplementedException();
    }

    protected override void InitializeShell(Window shell)
    {
      //throw new NotImplementedException();
    }

    protected override Window CreateShell()
    {
      return Container.Resolve<Concurrency>();
    }
  }

}
