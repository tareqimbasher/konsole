using System;

namespace Konsole
{
    internal class KonsoleScope : Konsole
    {
        internal KonsoleScope(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public override ConsoleColor ForegroundColor { get; set; }
        public override ConsoleColor BackgroundColor { get; set; }
    }
}
