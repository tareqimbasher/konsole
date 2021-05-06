using KonsoleDotNet.ProgressBars;
using System;
using System.Collections.Generic;

namespace KonsoleDotNet
{
    public static class ProgressBarExtensions
    {
        /// <summary>
        /// Creates a new progress bar.
        /// </summary>
        /// <param name="text">The text caption for the progress bar.</param>
        public static ProgressBar ProgressBar(this IKonsole konsole, string text)
        {
            return new ProgressBar(konsole, text, Console.CursorTop);
        }

        /// <summary>
        /// Creates a new progress bar in this group.
        /// </summary>
        /// <param name="text">The text caption for the progress bar.</param>
        public static ProgressBar ProgressBar(this ProgressBarGroup group, string text)
        {
            return group.Add(group.Konsole.ProgressBar(text));
        }

        /// <summary>
        /// Creates a progress bar group that can be used to group progress bars together.
        /// </summary>
        public static ProgressBarGroup ProgressBarGroup(this IKonsole konsole)
        {
            return new ProgressBarGroup(konsole);
        }

        /// <summary>
        /// Creates a progress bar group that can be used to group progress bars together.
        /// </summary>
        public static ProgressBarGroup ProgressBarGroup(this IKonsole konsole, params ProgressBar[] progressBars)
        {
            return new ProgressBarGroup(konsole, progressBars);
        }

        /// <summary>
        /// Creates a progress bar group that can be used to group progress bars together.
        /// </summary>
        public static ProgressBarGroup ProgressBarGroup(this IKonsole konsole, IEnumerable<ProgressBar> progressBars)
        {
            return new ProgressBarGroup(konsole, progressBars);
        }
    }
}
