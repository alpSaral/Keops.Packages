using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace EsGiris.Admin.WebGrid
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

        public static int AsInt(this string value) => AsInt(value, 0);

        public static int AsInt(this string value, int defaultValue) => int.TryParse(value, out int result) ? result : defaultValue;

        public static decimal AsDecimal(this string value) =>
            // Decimal.TryParse does not work consistently for some locales. For instance for lt-LT, it accepts but ignores decimal values so "12.12" is parsed as 1212.
            As<decimal>(value);

        public static decimal AsDecimal(this string value, decimal defaultValue) => As(value, defaultValue);

        public static float AsFloat(this string value) => AsFloat(value, default);

        public static float AsFloat(this string value, float defaultValue) => float.TryParse(value, out float result) ? result : defaultValue;

        public static DateTime AsDateTime(this string value) => AsDateTime(value, default);

        public static DateTime AsDateTime(this string value, DateTime defaultValue) => DateTime.TryParse(value, out DateTime result) ? result : defaultValue;

        public static TValue As<TValue>(this string value) => As(value, default(TValue));

        public static bool AsBool(this string value) => AsBool(value, default);

        public static bool AsBool(this string value, bool defaultValue) => bool.TryParse(value, out bool result) ? result : defaultValue;

        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }
                // try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch
            {
                // eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
            }
            return defaultValue;
        }

        public static bool IsBool(this string value) => bool.TryParse(value, out bool result);

        public static bool IsInt(this string value) => int.TryParse(value, out int result);

        public static bool IsDecimal(this string value) =>
            // For some reason, Decimal.TryParse incorrectly parses floating point values as decimal value for some cultures.
            // For example, 12.5 is parsed as 125 in lt-LT.
            Is<decimal>(value);

        public static bool IsFloat(this string value) => float.TryParse(value, out float result);

        public static bool IsDateTime(this string value) => DateTime.TryParse(value, out DateTime result);

        public static bool Is<TValue>(this string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null)
            {
                try
                {
                    if ((value == null) || converter.CanConvertFrom(null, value.GetType()))
                    {
                        // TypeConverter.IsValid essentially does this - a try catch - but uses InvariantCulture to convert. 
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        public static string ToHtmlString(this IHtmlContent helper)
        {
            using (var stringWriter = new StringWriter())
            {
                helper.WriteTo(stringWriter, System.Text.Encodings.Web.HtmlEncoder.Default);
                return stringWriter.ToString();
            }
        }
    }
}
