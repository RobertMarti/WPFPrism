using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VVM.Converters
{
    class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var isVisible = (bool) value;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visiblity = (Visibility) value;

            return visiblity == Visibility.Visible;
        }
    }
}

//TODO: TestProjekt
    ////[TestClass]
    ////public class BooleanToVisibilityConverterFixture
    ////{
    ////    [TestMethod]
    ////    public void ConvertWithNullValueReturnsCollapsed()
    ////    {
    ////        var converter = new BooleanToVisibilityConverter();
    ////        var value = converter.Convert(null, typeof(Visibility), null, CultureInfo.CurrentCulture);

    ////        Assert.AreEqual(Visibility.Collapsed, value);
    ////    }

    ////    [TestMethod]
    ////    public void ConvertWithFalseReturnsCollapsed()
    ////    {
    ////        var converter = new BooleanToVisibilityConverter();
    ////        var value = converter.Convert(false, typeof(Visibility), null, CultureInfo.CurrentCulture);

    ////        Assert.AreEqual(Visibility.Collapsed, value);
    ////    }

    ////    [TestMethod]
    ////    public void ConvertWithTrueReturnsVisible()
    ////    {
    ////        var converter = new BooleanToVisibilityConverter();
    ////        var value = converter.Convert(true, typeof(Visibility), null, CultureInfo.CurrentCulture);

    ////        Assert.AreEqual(Visibility.Visible, value);
    ////    }

    ////    [TestMethod]
    ////    public void ConvertBackWithVisibleReturnsTrue()
    ////    {
    ////        var converter = new BooleanToVisibilityConverter();
    ////        var value = converter.ConvertBack(Visibility.Visible, typeof(bool), null, CultureInfo.CurrentCulture);

    ////        Assert.AreEqual(true, value);
    ////    }

    ////    [TestMethod]
    ////    public void ConvertBackWithCollapsedReturnsFalse()
    ////    {
    ////        var converter = new BooleanToVisibilityConverter();
    ////        var value = converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, CultureInfo.CurrentCulture);

    ////        Assert.AreEqual(false, value);
    ////    }


