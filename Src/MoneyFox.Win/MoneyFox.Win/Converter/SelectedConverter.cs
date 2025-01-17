﻿namespace MoneyFox.Win.Converter;

using System;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

public class SelectedConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        return ((SelectionChangedEventArgs)value)?.AddedItems.FirstOrDefault();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
