using System.Globalization;

namespace RishUI
{
    public static class RishStringExtensions
    {
        public static string ToLower(this RishString str) => ToLowerInvariant(str);
        public static string ToLower(this RishString str, CultureInfo cultureInfo)
        {
            if (str.IsEmpty)
            {
                return string.Empty;
            }

            return str.value.ToLower(cultureInfo);
        }
        public static string ToLowerInvariant(this RishString str)
        {
            if (str.IsEmpty)
            {
                return string.Empty;
            }

            return str.value.ToLowerInvariant();
        }

        public static string ToUpper(this RishString str) => ToUpperInvariant(str);
        public static string ToUpper(this RishString str, CultureInfo cultureInfo)
        {
            if (str.IsEmpty)
            {
                return string.Empty;
            }

            return str.value.ToUpper(cultureInfo);
        }
        public static string ToUpperInvariant(this RishString str)
        {
            if (str.IsEmpty)
            {
                return string.Empty;
            }

            return str.value.ToUpperInvariant();
        }
        
        public static bool Contains(this RishString str, char value) => !str.IsEmpty && str.value.Contains(value);
        public static bool Contains(this RishString str, string value) => !str.IsEmpty && str.value.Contains(value);
        public static bool Contains(this RishString str, char value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return ignoreCase
                ? str.ToLowerInvariant().Contains(char.ToLowerInvariant(value))
                : str.Contains(value);
        }
        public static bool Contains(this RishString str, string value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return ignoreCase
                ? str.ToLowerInvariant().Contains(value.ToLowerInvariant())
                : str.Contains(value);
        }
        public static bool Contains(this RishString str, char value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return ignoreCase
                ? str.ToLower(culture).Contains(char.ToLower(value, culture))
                : str.Contains(value);
        }
        public static bool Contains(this RishString str, string value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return ignoreCase
                ? str.ToLower(culture).Contains(value.ToLower(culture))
                : str.Contains(value);
        }
        public static bool ContainsIgnoreCase(this RishString str, char value) => Contains(str, value, true);
        public static bool ContainsIgnoreCase(this RishString str, string value) => Contains(str, value, true);

        public static bool StartsWith(this RishString str, char value)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return str.value.StartsWith(value);
        }
        public static bool StartsWith(this RishString str, string value)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return str.value.StartsWith(value);
        }
        public static bool StartsWith(this RishString str, char value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLowerInvariant().StartsWith(char.ToLowerInvariant(value))
                : str.StartsWith(value);
        }
        public static bool StartsWith(this RishString str, string value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLowerInvariant().StartsWith(value.ToLowerInvariant())
                : str.StartsWith(value);
        }
        public static bool StartsWith(this RishString str, char value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLower(culture).StartsWith(char.ToLower(value))
                : str.StartsWith(value);
        }
        public static bool StartsWith(this RishString str, string value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLower(culture).StartsWith(value.ToLower(culture))
                : str.StartsWith(value);
        }
        public static bool StartsWithIgnoreCase(this RishString str, char value) => StartsWith(str, value, true);
        public static bool StartsWithIgnoreCase(this RishString str, string value) => StartsWith(str, value, true);

        public static bool EndsWith(this RishString str, char value)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return str.value.EndsWith(value);
        }
        public static bool EndsWith(this RishString str, string value)
        {
            if (str.IsEmpty)
            {
                return false;
            }

            return str.value.EndsWith(value);
        }
        public static bool EndsWith(this RishString str, char value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLowerInvariant().EndsWith(char.ToLowerInvariant(value))
                : str.EndsWith(value);
        }
        public static bool EndsWith(this RishString str, string value, bool ignoreCase)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLowerInvariant().EndsWith(value.ToLowerInvariant())
                : str.EndsWith(value);
        }
        public static bool EndsWith(this RishString str, char value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLower(culture).EndsWith(char.ToLower(value))
                : str.EndsWith(value);
        }
        public static bool EndsWith(this RishString str, string value, bool ignoreCase, CultureInfo culture)
        {
            if (str.IsEmpty)
            {
                return false;
            }
            
            return ignoreCase
                ? str.ToLower(culture).EndsWith(value.ToLower(culture))
                : str.EndsWith(value);
        }
        public static bool EndsWithIgnoreCase(this RishString str, char value) => EndsWith(str, value, true);
        public static bool EndsWithIgnoreCase(this RishString str, string value) => EndsWith(str, value, true);
        
        public static RishString Remove(this RishString str, int startIndex, int count)
        {
            if (str.IsEmpty)
            {
                return str;
            }

            return str.value.Remove(startIndex, count);
        }
        public static RishString Remove(this RishString str, int startIndex)
        {
            if (str.IsEmpty)
            {
                return str;
            }

            return str.value.Remove(startIndex);
        }
    }
}
