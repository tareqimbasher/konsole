using KonsoleDotNet.Transcripts;
using System;

namespace KonsoleDotNet
{
    /// <summary>
    /// Default implementation of <see cref="IKonsole"/>.
    /// </summary>
    public class Konsole : IKonsole
    {
        private static readonly Lazy<Konsole> _lazy = new Lazy<Konsole>(() => new Konsole());
        public static Konsole Default => _lazy.Value;
        
        private ITranscript _transcript;

        /// <summary>
        /// Constructs an instance of the <see cref="Konsole"/> with built-in defaults.
        /// </summary>
        public Konsole()
        {
            _transcript = new Transcript();
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

        /// <inheritdoc />
        public virtual ConsoleColor ForegroundColor { get; set; }

        /// <inheritdoc />
        public virtual ConsoleColor BackgroundColor { get; set; }

        /// <inheritdoc />
        public virtual KonsoleDefaults Defaults { get; }

        /// <inheritdoc />
        public virtual ITranscript Transcript => _transcript;
        
        /// <inheritdoc />
        public bool TranscriptLoggingEnabled { get; private set; }

        /// <inheritdoc />
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

            _transcript.Add(text, TranscriptLogType.Output, now);

            // Safe guard against exceptions in user-defined PostWriteAction
            try
            {
                Defaults.PostWriteAction?.Invoke(this, text);
            }
            catch
            {
            }

            return this;
        }


        /// <inheritdoc />
        public virtual IKonsole Info(string text)
        {
            Defaults.Info?.Invoke(this.CreateScope(), text);
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole Debug(string text)
        {
            Defaults.Debug?.Invoke(this.CreateScope(), text);
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole Warn(string text)
        {
            Defaults.Warn?.Invoke(this.CreateScope(), text);
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole Error(string text)
        {
            Defaults.Error?.Invoke(this.CreateScope(), text);
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole StartTranscriptLogging()
        {
            TranscriptLoggingEnabled = true;
            return this;
        }
        
        /// <inheritdoc />
        public virtual IKonsole StartTranscriptLogging(ITranscript transcript)
        {
            _transcript = transcript ?? throw new ArgumentNullException(nameof(transcript));
            TranscriptLoggingEnabled = true;
            return this;
        }

        /// <inheritdoc />
        public virtual IKonsole StopTranscriptLogging()
        {
            TranscriptLoggingEnabled = false;
            return this;
        }
    }
}