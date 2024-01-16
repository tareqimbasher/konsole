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
            DefaultSettings = new KonsoleDefaults
            {
                DefaultForegroundColor = konsole.DefaultSettings.DefaultForegroundColor,
                DefaultBackgroundColor = konsole.DefaultSettings.DefaultBackgroundColor,
                PostWriteAction = konsole.DefaultSettings.PostWriteAction
            };
        }

        public IKonsole Konsole { get; }

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public KonsoleDefaults DefaultSettings { get; }

        public ITranscript Transcript => Konsole.Transcript;

        public bool TranscriptLoggingEnabled => Konsole.TranscriptLoggingEnabled;

        public IKonsole Write(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
            => Konsole.Write(text, foregroundColor ?? ForegroundColor, backgroundColor ?? BackgroundColor);

        public IKonsole ResetColors()
        {
            ForegroundColor = DefaultSettings.DefaultForegroundColor;
            BackgroundColor = DefaultSettings.DefaultBackgroundColor;
            return this;
        }

        public IKonsole StartTranscriptLogging() => Konsole.StartTranscriptLogging();

        public IKonsole StartTranscriptLogging(ITranscript transcript) => Konsole.StartTranscriptLogging(transcript);

        public IKonsole StopTranscriptLogging() => Konsole.StopTranscriptLogging();
    }
}