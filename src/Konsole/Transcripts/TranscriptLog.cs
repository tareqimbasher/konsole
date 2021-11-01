using System;

namespace KonsoleDotNet.Transcripts
{
    /// <summary>
    /// Represents a single entry of a <see cref="ITranscript"/>.
    /// </summary>
    public class TranscriptLog
    {
        public TranscriptLog(string text, TranscriptLogType type, DateTime dateTimeUtc)
        {
            Text = text;
            Type = type;
            DateTimeUtc = dateTimeUtc;
        }

        /// <summary>
        /// The text that was logged.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The type of this log.
        /// </summary>
        public TranscriptLogType Type { get; set; }

        /// <summary>
        /// The date and time the text was captured.
        /// </summary>
        public DateTime DateTimeUtc { get; set; }
    }
}