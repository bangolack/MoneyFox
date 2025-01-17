﻿namespace MoneyFox.Win.Converter;

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, string language)
    {
        if (parameter != null && parameter.ToString() == "revert")
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
