using System.Globalization;

namespace RishUI
{
    public static class RishStringExtensions
    {
        public static bool Contains(this RishString str, string value) => !str.IsEmpty && str.value.Contains(value);
        public static string ToLower(this RishString str)
        {
            if (str.IsEmpty)
            {
                return string.Empty;
            }

            return str.value.ToLower();
        }
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
    }
}
