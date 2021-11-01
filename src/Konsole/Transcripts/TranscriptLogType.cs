using System;
using System.Collections.Generic;
using System.Text;

namespace KonsoleDotNet.Transcripts
{
    /// <summary>
    /// Types of transcript logs.
    /// </summary>
    public enum TranscriptLogType
    {
        /// <summary>
        /// Input that is read into a <see cref="IKonsole"/>.
        /// </summary>
        Input = 1,

        /// <summary>
        /// Output written to a <see cref="IKonsole"/>.
        /// </summary>
        Output = 2
    }
}