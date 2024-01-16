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
            DefaultSettings = new KonsoleDefaults();
            ForegroundColor = DefaultSettings.DefaultForegroundColor;
            BackgroundColor = DefaultSettings.DefaultBackgroundColor;
        }

        /// <summary>
        /// Constructs an instance of the <see cref="Konsole"/> with the specified defaults.
        /// </summary>
        /// <param name="defaultSettings">Default options</param>
        public Konsole(KonsoleDefaults defaultSettings) : this()
        {
            DefaultSettings = defaultSettings;
        }

        public virtual ConsoleColor ForegroundColor { get; set; }

        public virtual ConsoleColor BackgroundColor { get; set; }

        public virtual KonsoleDefaults DefaultSettings { get; }

        public virtual ITranscript Transcript => _transcript;

        public bool TranscriptLoggingEnabled { get; private set; }

        public virtual IKonsole Write(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            var now = DateTime.UtcNow;

            var originalForegroundColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor ?? ForegroundColor;
            Console.BackgroundColor = backgroundColor ?? BackgroundColor;
            Console.Write(text);
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;

            _transcript.Add(text, TranscriptLogType.Output, now);

            try
            {
                DefaultSettings.PostWriteAction?.Invoke(this, text);
            }
            catch
            {
                // Ignore exceptions in user-defined PostWriteAction
            }

            return this;
        }

        public virtual IKonsole ResetColors()
        {
            ForegroundColor = DefaultSettings.DefaultForegroundColor;
            BackgroundColor = DefaultSettings.DefaultBackgroundColor;
            return this;
        }

        public virtual IKonsole StartTranscriptLogging()
        {
            TranscriptLoggingEnabled = true;
            return this;
        }

        public virtual IKonsole StartTranscriptLogging(ITranscript transcript)
        {
            _transcript = transcript ?? throw new ArgumentNullException(nameof(transcript));
            TranscriptLoggingEnabled = true;
            return this;
        }

        public virtual IKonsole StopTranscriptLogging()
        {
            TranscriptLoggingEnabled = false;
            return this;
        }
    }
}