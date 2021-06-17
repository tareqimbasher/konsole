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

            Info = (console, text) => console.WriteLine(text, ConsoleColor.White);
            Debug = (console, text) => console.WriteLine(text, ConsoleColor.Green);
            Warn = (console, text) => console.WriteLine(text, ConsoleColor.Yellow);
            Error = (console, text) => console.WriteLine(text, ConsoleColor.Red);

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
        /// The default action that is executed when an Info value is written to the console. Writes white text by default.
        /// </summary>
        public Action<IKonsole, string> Info { get; set; }

        /// <summary>
        /// The default action that is executed when a Debug value is written to the console. Writes green text by default.
        /// </summary>
        public Action<IKonsole, string> Debug { get; set; }

        /// <summary>
        /// The default action that is executed when an Warning value is written to the console. Writes yellow text by default.
        /// </summary>
        public Action<IKonsole, string> Warn { get; set; }

        /// <summary>
        /// The default action that is executed when an Error value is written to the console. Writes red text by default.
        /// </summary>
        public Action<IKonsole, string> Error { get; set; }

        /// <summary>
        /// An action that is executed after a value is written to the console. No action is executed by default.
        /// </summary>
        public Action<IKonsole, string> PostWriteAction { get; set; }
    }
}
