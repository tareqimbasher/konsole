namespace KonsoleDotNet
{
    internal static class InternalExtensions
    {
        internal static string Truncate(this string str, int maxLength)
        {
            if (str == null)
                return str;

            if (str.Length <= maxLength)
                return str;
            return str.Substring(0, maxLength);
        }
    }
}
