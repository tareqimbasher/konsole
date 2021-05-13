using System;

namespace KonsoleDotNet.Transcripts
{
    public class TranscriptLog
    {
        public TranscriptLog(string text, TranscriptLogType type, DateTime dateTime)
        {
            Text = text;
            Type = type;
            DateTime = dateTime;
        }

        public string Text { get; set; }
        public TranscriptLogType Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}
