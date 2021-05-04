using System;
using System.Collections.Generic;
using System.Linq;

namespace Konsole.ProgressBars
{
    public class ProgressBarGroup
    {
        private readonly List<ProgressBar> _progressBars;

        internal ProgressBarGroup(IKonsole konsole)
        {
            Konsole = konsole ?? throw new ArgumentNullException(nameof(konsole));
            _progressBars = new List<ProgressBar>();
        }

        internal ProgressBarGroup(IKonsole konsole, IEnumerable<ProgressBar> progressBars) : this(konsole)
        {
            foreach (var progressBar in progressBars ?? throw new ArgumentNullException(nameof(progressBars)))
            {
                Add(progressBar);
            }
        }

        internal IKonsole Konsole { get; set; }

        /// <summary>
        /// Adds the specified progress bar to this group.
        /// </summary>
        public ProgressBar Add(ProgressBar progressBar)
        {
            SetRow(progressBar);
            _progressBars.Add(progressBar);
            return progressBar;
        }

        /// <summary>
        /// Sets the row the progress bar will write text to.
        /// </summary>
        private void SetRow(ProgressBar progressBar)
        {
            int row = _progressBars.Any() ? (_progressBars.Max(p => p.Row) + 1) : Console.CursorTop;
            progressBar.Row = row;
        }
    }
}
