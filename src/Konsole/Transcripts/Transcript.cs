using System;
using System.Collections.Generic;
using System.Text;

namespace KonsoleDotNet.Transcripts
{
    /// <summary>
    /// Default implementation of a <see cref="ITranscript"/>.
    /// </summary>
    public class Transcript : ITranscript
    {
        private readonly List<TranscriptLog> _logs;

        public Transcript()
        {
            _logs = new List<TranscriptLog>();
        }

        public Transcript(string name) : this()
        {
            Name = name;
        }

        public event EventHandler<TranscriptLog> LogAdded;

        public string Name { get; }

        public IReadOnlyList<TranscriptLog> Logs => _logs;
        public bool IsReadOnly { get; private set; } = false;

        public virtual void Add(string text, TranscriptLogType type, DateTime dateTimeUtc)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This transcript is readonly and cannot be added to.");

            var log = new TranscriptLog(text, type, dateTimeUtc);
            _logs.Add(log);
            OnLogAdded(log);
        }

        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This transcript is readonly and cannot be cleared.");

            _logs.Clear();
        }

        public void MakeReadOnly()
        {
            IsReadOnly = true;
        }

        protected virtual void OnLogAdded(TranscriptLog log)
        {
            LogAdded?.Invoke(this, log);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var log in Logs)
            {
                sb.Append(log.Text);
            }

            return sb.ToString();
        }
    }
}