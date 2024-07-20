﻿using System.Globalization;
using System;
using System.Windows;
using System.Windows.Data;

namespace YoutubeDownloader.Converters
{
    [ValueConversion(typeof(bool),typeof(Visibility))]
    public class BooleanToVisibilityConverter:IValueConverter
    {
        public bool IsInversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInversed)
                return value != null && bool.Parse(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;

            return value != null && bool.Parse(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
