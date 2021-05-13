using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KonsoleDotNet.Transcripts
{
    public class TranscriptCollection
    {
        private static TranscriptCollection _default;
        public static TranscriptCollection Default
        {
            get
            {
                if (_default == null)
                    _default = new TranscriptCollection();
                return _default;
            }
        }

        protected readonly Dictionary<string, Transcript> _transcripts;

        public TranscriptCollection()
        {
            _transcripts = new Dictionary<string, Transcript>();
        }

        public IReadOnlyList<Transcript> All => _transcripts.Values.ToList();

        public bool Contains(string transcriptName) => _transcripts.ContainsKey(transcriptName);

        public virtual Transcript GetOrCreate(string transcriptName)
        {
            if (_transcripts.TryGetValue(transcriptName, out var transcript))
                return transcript;

            transcript = new Transcript(transcriptName);
            _transcripts.Add(transcriptName, transcript);

            return transcript;
        }

        public virtual Transcript Remove(string transcriptName)
        {
            if (_transcripts.TryGetValue(transcriptName, out var transcript))
            {
                _transcripts.Remove(transcriptName);
                return transcript;
            }

            return null;
        }
    }
}
