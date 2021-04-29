using System;

namespace Konsole
{
    /// <summary>
    /// A utility wrapper of the <see cref="Console"/>.
    /// </summary>
    public class Konsole
    {
        /// <summary>
        /// Constructs an instance of the <see cref="Konsole"/> with built-in defaults.
        /// </summary>
        public Konsole()
        {
            Defaults = new KonsoleDefaults();
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
        }

        /// <summary>
        /// Constructs an instance of the <see cref="Konsole"/> with the specified defaults.
        /// </summary>
        /// <param name="defaults">Default options</param>
        public Konsole(KonsoleDefaults defaults) : this()
        {
            Defaults = defaults;
        }

        /// <summary>
        /// Gets or sets the foreground color of this console; that is, the color of the text printed by this console.
        /// </summary>
        public virtual ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of this console.
        /// </summary>
        public virtual ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The defaults this console uses.
        /// </summary>
        public virtual KonsoleDefaults Defaults { get; }

        /// <summary>
        /// Writes the specified string to the console with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        public virtual Konsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var originalForegroundColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;
            Defaults.PostWriteAction?.Invoke(this, text);
            return this;
        }

        /// <summary>
        /// Writes the specified string to the console with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        public virtual Konsole Write(string text, ConsoleColor foregroundColor) => Write(text, foregroundColor, BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole Write(string text) => Write(text, ForegroundColor, BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        public virtual Konsole WriteLine(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Write(text, foregroundColor, backgroundColor);
            Console.WriteLine();
            return this;
        }

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        public virtual Konsole WriteLine(string text, ConsoleColor foregroundColor) => WriteLine(text, foregroundColor, BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole WriteLine(string text) => WriteLine(text, ForegroundColor, BackgroundColor);

        /// <summary>
        /// Writes a line terminator to the console.
        /// </summary>
        public virtual Konsole WriteLine()
        {
            Console.WriteLine();
            Defaults.PostWriteAction?.Invoke(this, string.Empty);
            return this;
        }

        /// <summary>
        /// Writes text as info output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole Info(string text)
        {
            Defaults.Info(this.CreateScope(), text);
            return this;
        }

        /// <summary>
        /// Writes text as debug output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole Debug(string text)
        {
            Defaults.Debug(this.CreateScope(), text);
            return this;
        }

        /// <summary>
        /// Writes text as warn output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole Warn(string text)
        {
            Defaults.Warn(this.CreateScope(), text);
            return this;
        }

        /// <summary>
        /// Writes text as error output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public virtual Konsole Error(string text)
        {
            Defaults.Error(this.CreateScope(), text);
            return this;
        }

        /// <summary>
        /// Resets this console's colors to their default setting.
        /// </summary>
        public virtual Konsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }
    }
}
