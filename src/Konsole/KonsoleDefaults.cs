using System;

namespace KonsoleDotNet
{
    /// <summary>
    /// Represents the default settings of an <see cref="IKonsole"/> instance.
    /// </summary>
    public class KonsoleDefaults
    {
        /// <summary>
        /// Instantiates a <see cref="KonsoleDefaults"/> instance.
        /// </summary>
        public KonsoleDefaults()
        {
            DefaultForegroundColor = Console.ForegroundColor;
            DefaultBackgroundColor = Console.BackgroundColor;
            PostWriteAction = (console, text) => { };
        }

        /// <summary>
        /// The default foreground color. The <see cref="Console"/> foreground color is used by default.
        /// </summary>
        public ConsoleColor DefaultForegroundColor { get; set; }

        /// <summary>
        /// The default background color. The <see cref="Console"/> background color is used by default.
        /// </summary>
        public ConsoleColor DefaultBackgroundColor { get; set; }

        /// <summary>
        /// An action that is executed after a value is written to the console. No action is executed by default.
        /// </summary>
        public Action<IKonsole, string> PostWriteAction { get; set; }
    }
}