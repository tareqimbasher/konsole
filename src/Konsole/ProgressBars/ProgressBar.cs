using System;

namespace KonsoleDotNet.ProgressBars
{
    /// <summary>
    /// A bar that represents progress.
    /// </summary>
    public class ProgressBar
    {
        internal ProgressBar(IKonsole konsole, string text, int row)
        {
            Konsole = konsole;
            Text = text;
            Row = row;
        }

        internal ProgressBar(IKonsole konsole, string text) : this(konsole, text, Console.CursorTop)
        {
        }

        internal IKonsole Konsole { get; }
        internal int Row { get; set; }

        /// <summary>
        /// The text caption for this progress bar.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Update the progress of this progress bar.
        /// </summary>
        /// <param name="percentComplete">The percent complete as an integer.</param>
        public void Update(int percentComplete)
        {
            Update(percentComplete, Text);
        }

        /// <summary>
        /// Update the progress and text caption of this progress bar.
        /// </summary>
        /// <param name="percentComplete">The percent complete as an integer.</param>
        /// <param name="text">The text caption to display.</param>
        public void Update(int percentComplete, string text)
        {
            lock (Konsole)
            {
                if (percentComplete > 100)
                    percentComplete = 100;

                Text = text;
                if (Text == null)
                    Text = string.Empty;

                Console.SetCursorPosition(0, Row);

                int half = (int)Math.Floor((Console.BufferWidth - 1) / 2.0);

                Konsole
                    .ClearCurrentLine()
                    .Write(Text.Truncate(half).PadRight(half))
                    .WithForeColor(ConsoleColor.White).Write($" {percentComplete,-3}% ");

                int availableSpace = Console.BufferWidth - Console.CursorLeft - 1;
                int barWidth = (int)Math.Ceiling((percentComplete / 100.0) * availableSpace);

                Konsole.Write(new string('#', barWidth), ConsoleColor.Green).WriteLine();
            }
        }
    }
}
