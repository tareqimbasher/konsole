using System;

namespace Konsole
{
    /// <summary>
    /// Represents the default settings of a <see cref="Konsole"/> instance.
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

            Info = (console, text) => console.WithForeColor(ConsoleColor.White).WriteLine(text);
            Debug = (console, text) => console.WithForeColor(ConsoleColor.Green).WriteLine(text);
            Warn = (console, text) => console.WithForeColor(ConsoleColor.Yellow).WriteLine(text);
            Error = (console, text) => console.WithForeColor(ConsoleColor.Red).WriteLine(text);

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
        public Action<Konsole, string> Info { get; set; }

        /// <summary>
        /// The default action that is executed when a Debug value is written to the console. Writes green text by default.
        /// </summary>
        public Action<Konsole, string> Debug { get; set; }

        /// <summary>
        /// The default action that is executed when an Warning value is written to the console. Writes yellow text by default.
        /// </summary>
        public Action<Konsole, string> Warn { get; set; }

        /// <summary>
        /// The default action that is executed when an Error value is written to the console. Writes red text by default.
        /// </summary>
        public Action<Konsole, string> Error { get; set; }

        /// <summary>
        /// An action that is executed after a value is written to the console. No action is executed by default.
        /// </summary>
        public Action<Konsole, string> PostWriteAction { get; set; }
    }
}
