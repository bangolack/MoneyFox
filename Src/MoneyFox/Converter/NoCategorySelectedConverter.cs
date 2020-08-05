﻿using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Categories;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class NoCategorySelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = (CategoryViewModel)value;

            return string.IsNullOrEmpty(category?.Name)
                   ? Strings.SelectCategoryTitle : category.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
