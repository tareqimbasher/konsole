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

        public IKonsole Konsole { get; }

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public KonsoleDefaults Defaults { get; }

        public ITranscript Transcript => Konsole.Transcript;

        public IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            => Konsole.Write(text, foregroundColor, backgroundColor);

        public IKonsole Debug(string text) => Konsole.Debug(text);
        public IKonsole Info(string text) => Konsole.Info(text);
        public IKonsole Warn(string text) => Konsole.Warn(text);
        public IKonsole Error(string text) => Konsole.Error(text);

        public IKonsole ResetColors()
        {
            ForegroundColor = Defaults.DefaultForegroundColor;
            BackgroundColor = Defaults.DefaultBackgroundColor;
            return this;
        }

        public IKonsole StartTranscriptLogging(ITranscript transcript) => Konsole.StartTranscriptLogging(transcript);

        public IKonsole StopTranscriptLogging() => Konsole.StopTranscriptLogging();
    }
}
