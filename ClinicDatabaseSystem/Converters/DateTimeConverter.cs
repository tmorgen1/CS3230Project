using System;
using Windows.UI.Xaml.Data;

namespace ClinicDatabaseSystem.Converters
{
    /// <summary>
    /// Handles converting date time to formatted strings.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class DateTimeConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified from date time to formatted string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>string formatted date</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            var dateTime = DateTime.Parse(value.ToString());
            return dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Converts the string back to datetime object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>datetime object from string</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            return DateTime.Parse(value.ToString());
        }
    }
}
