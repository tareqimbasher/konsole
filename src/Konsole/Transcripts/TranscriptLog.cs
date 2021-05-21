using System;

namespace KonsoleDotNet.Transcripts
{
    public class TranscriptLog
    {
        public TranscriptLog(string text, TranscriptLogType type, DateTime dateTimeUtc)
        {
            Text = text;
            Type = type;
            DateTimeUtc = dateTimeUtc;
        }

        public string Text { get; set; }
        public TranscriptLogType Type { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
