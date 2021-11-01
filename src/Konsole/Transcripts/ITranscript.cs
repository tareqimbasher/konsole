using System;
using System.Collections.Generic;

namespace KonsoleDotNet.Transcripts
{
    /// <summary>
    /// Represents a collection of inputs and outputs captured by a <see cref="IKonsole"/>.
    /// </summary>
    public interface ITranscript
    {
        /// <summary>
        /// Transcript name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Log entries.
        /// </summary>
        IReadOnlyList<TranscriptLog> Logs { get; }

        /// <summary>
        /// Event is fired every time a log is added to this transcript.
        /// </summary>
        event EventHandler<TranscriptLog> LogAdded;

        /// <summary>
        /// Indicates if this transcript is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Adds a log to this transcript.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="type">The type of log to add.</param>
        /// <param name="dateTimeUtc">The date and time the text was captured.</param>
        void Add(string text, TranscriptLogType type, DateTime dateTimeUtc);

        /// <summary>
        /// Clears all logs from this transcript.
        /// </summary>
        void Clear();

        /// <summary>
        /// Makes this transcript read-only if it is not already. No further modifications will be allowed to 
        /// this transcript after this method is called. This cannot be undone.
        /// </summary>
        void MakeReadOnly();
    }
}