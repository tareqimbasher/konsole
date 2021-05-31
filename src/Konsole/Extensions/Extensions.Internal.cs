﻿namespace KonsoleDotNet
{
    internal static class InternalExtensions
    {
        internal static string Truncate(this string str, int maxLength)
        {
            if (str == null || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength);
        }
    }
}
