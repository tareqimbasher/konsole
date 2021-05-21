using System;
using System.Collections.Generic;
using System.Text;

namespace KonsoleDotNet.Transcripts
{
    public class Transcript : ITranscript
    {
        private readonly string _name;
        private readonly List<TranscriptLog> _logs;
        private bool _isReadOnly = false;

        public Transcript()
        {
            _logs = new List<TranscriptLog>();
        }

        public Transcript(string name) : this()
        {
            _name = name;
        }

        public event EventHandler<TranscriptLog> LogAdded;

        public string Name => _name;
        public IReadOnlyList<TranscriptLog> Logs => _logs;
        public bool IsReadOnly => _isReadOnly;

        public virtual void Add(string text, TranscriptLogType type, DateTime dateTimeUtc)
        {
            if (_isReadOnly)
                throw new InvalidOperationException("This transcript is readonly and cannot be added to.");

            var log = new TranscriptLog(text, type, dateTimeUtc);
            _logs.Add(log);
            OnLogAdded(log);
        }

        public void Clear()
        {
            if (_isReadOnly)
                throw new InvalidOperationException("This transcript is readonly and cannot be cleared.");

            _logs.Clear();
        }

        public void MakeReadOnly()
        {
            _isReadOnly = true;
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
