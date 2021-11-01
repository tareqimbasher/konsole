using KonsoleDotNet.Transcripts;
using System;

namespace KonsoleDotNet
{
    /// <summary>
    /// Default implementation of <see cref="IKonsoleScope"/>.
    /// </summary>
    public class KonsoleScope : IKonsoleScope
    {
        /// <summary>
        /// Initializes an instance of <see cref="KonsoleScope"/>.
        /// </summary>
        /// <param name="konsole"></param>
        public KonsoleScope(IKonsole konsole)
        {
            Konsole = konsole;

            ForegroundColor = konsole.ForegroundColor;
            BackgroundColor = konsole.BackgroundColor;
            Defaults = new KonsoleDefaults
            {
                DefaultForegroundColor = konsole.Defaults.DefaultForegroundColor,
                DefaultBackgroundColor = konsole.Defaults.DefaultBackgroundColor,
                Debug = konsole.Defaults.Debug,
                Info = konsole.Defaults.Info,
                Warn = konsole.Defaults.Warn,
                Error = konsole.Defaults.Error,
                PostWriteAction = konsole.Defaults.PostWriteAction
            };
        }

        /// <inheritdoc />
        public IKonsole Konsole { get; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor { get; set; }

        /// <inheritdoc />
        public ConsoleColor BackgroundColor { get; set; }

        /// <inheritdoc />
        public KonsoleDefaults Defaults { get; }

        /// <inheritdoc />
        public ITranscript Transcript => Konsole.Transcript;

        /// <inheritdoc />
        public bool TranscriptLoggingEnabled => Konsole.TranscriptLoggingEnabled;

        /// <inheritdoc />
        public IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            => Konsole.Write(text, foregroundColor, backgroundColor);

        /// <inheritdoc />
        public IKonsole Debug(string text) => Konsole.Debug(text);
        /// <inheritdoc />
        public IKonsole Info(string text) => Konsole.Info(text);
        /// <inheritdoc />
        public IKonsole Warn(string text) => Konsole.Warn(text);
        /// <inheritdoc />
        public IKonsole Error(string text) => Konsole.Error(text);

        /// <inheritdoc />
        public IKonsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }

        /// <inheritdoc />
        public IKonsole StartTranscriptLogging() => Konsole.StartTranscriptLogging();
        
        /// <inheritdoc />
        public IKonsole StartTranscriptLogging(ITranscript transcript) => Konsole.StartTranscriptLogging(transcript);

        /// <inheritdoc />
        public IKonsole StopTranscriptLogging() => Konsole.StopTranscriptLogging();
    }
}