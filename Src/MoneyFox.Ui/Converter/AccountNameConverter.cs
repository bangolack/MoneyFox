﻿namespace MoneyFox.Ui.Converter;

using System.Globalization;
using MoneyFox.Core.Common.Helpers;
using ViewModels.Accounts;

public class AccountNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(value is AccountViewModel account)
            ? string.Empty
            : $"{account.Name} ({account.CurrentBalance.ToString(format: "C", provider: CultureHelper.CurrentCulture)})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
