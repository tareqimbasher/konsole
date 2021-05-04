﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    internal static class InternalExtensions
    {
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
                return str;

            if (str.Length <= maxLength)
                return str;
            return str.Substring(0, maxLength);
        }
    }
}
