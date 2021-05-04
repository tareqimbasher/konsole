using System;

namespace Konsole
{
    /// <summary>
    /// A utility wrapper of the <see cref="Console"/>.
    /// </summary>
    public class Konsole : IKonsole
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

        public virtual ConsoleColor ForegroundColor { get; set; }

        public virtual ConsoleColor BackgroundColor { get; set; }

        public virtual KonsoleDefaults Defaults { get; }


        public virtual IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
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

        public IKonsole Write(string text, ConsoleColor foregroundColor) => Write(text, foregroundColor, BackgroundColor);

        public IKonsole Write(string text) => Write(text, ForegroundColor, BackgroundColor);


        public IKonsole WriteLine(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor) => Write(text + "\n", foregroundColor, backgroundColor);

        public IKonsole WriteLine(string text, ConsoleColor foregroundColor) => WriteLine(text, foregroundColor, BackgroundColor);

        public IKonsole WriteLine(string text) => WriteLine(text, ForegroundColor, BackgroundColor);

        public IKonsole WriteLine() => Write("\n");


        public virtual IKonsole Info(string text)
        {
            Defaults.Info(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Debug(string text)
        {
            Defaults.Debug(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Warn(string text)
        {
            Defaults.Warn(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Error(string text)
        {
            Defaults.Error(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }
    }
}
