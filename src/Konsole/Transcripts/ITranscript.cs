using System;
using System.Collections.Generic;

namespace KonsoleDotNet.Transcripts
{
    public interface ITranscript
    {
        event EventHandler<TranscriptLog> LogAdded;
        string Name { get; }
        IReadOnlyList<TranscriptLog> Logs { get; }
        void Add(string text, TranscriptLogType type, DateTime dateTime);
        void Clear();
        void MakeReadOnly();
    }
}
