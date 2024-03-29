﻿using System;

namespace KonsoleDotNet
{
    public static class KonsoleScopeExtensions
    {
        /// <summary>
        /// Creates a scope where configuration changes to the scope do not affect this <see cref="IKonsole"/> instance.
        /// </summary>
        /// <param name="konsole">The <see cref="IKonsole"/> to create a scope for.</param>
        public static IKonsoleScope CreateScope(this IKonsole konsole)
        {
            return new KonsoleScope(konsole);
        }

        /// <summary>
        /// Sets the colors to use within a scope.
        /// </summary>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color.</param>
        public static IKonsoleScope WithColors(this IKonsole konsole, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (!(konsole is IKonsoleScope scope))
            {
                scope = new KonsoleScope(konsole);
            }
            
            scope.ForegroundColor = foregroundColor;
            scope.BackgroundColor = backgroundColor;
            return scope;
        }

        /// <summary>
        /// Sets the foreground color within a scope.
        /// </summary>
        public static IKonsoleScope WithForeColor(this IKonsole konsole, ConsoleColor foregroundColor)
        {
            return WithColors(konsole, foregroundColor, konsole.BackgroundColor);
        }

        /// <summary>
        /// Sets the background color within a scope.
        /// </summary>
        public static IKonsoleScope WithBackColor(this IKonsole konsole, ConsoleColor backgroundColor)
        {
            return WithColors(konsole, konsole.ForegroundColor, backgroundColor);
        }
    }
}