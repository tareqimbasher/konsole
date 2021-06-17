using KonsoleDotNet.Transcripts;
using System;

namespace KonsoleDotNet
{
    /// <summary>
    /// Default implementation of <see cref="IKonsole"/>.
    /// </summary>
    public class Konsole : IKonsole
    {
        private ITranscript _transcript;

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

        public virtual ITranscript Transcript { get; }


        public virtual IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var now = DateTime.UtcNow;

            var originalForegroundColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;

            _transcript?.Add(text, TranscriptLogType.Output, now);

            // Safe gaurd against exceptions in user-defined PostWriteAction
            try
            {
                Defaults.PostWriteAction?.Invoke(this, text);
            }
            catch { }

            return this;
        }


        public virtual IKonsole Info(string text)
        {
            Defaults.Info?.Invoke(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Debug(string text)
        {
            Defaults.Debug?.Invoke(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Warn(string text)
        {
            Defaults.Warn?.Invoke(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole Error(string text)
        {
            Defaults.Error?.Invoke(this.CreateScope(), text);
            return this;
        }

        public virtual IKonsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }


        public virtual IKonsole StartTranscriptLogging(ITranscript transcript)
        {
            _transcript = transcript ?? throw new ArgumentNullException(nameof(transcript));
            return this;
        }

        public virtual IKonsole StopTranscriptLogging()
        {
            _transcript = null;
            return this;
        }
    }
}
