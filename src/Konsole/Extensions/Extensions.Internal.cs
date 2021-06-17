namespace KonsoleDotNet
{
    internal static class InternalExtensions
    {
        /// <summary>
        /// Truncates a string to the specified max length. If string is less than the specified max length,
        /// the same string is returned.
        /// </summary>
        internal static string Truncate(this string str, int maxLength)
        {
            if (str == null || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength);
        }
    }
}
