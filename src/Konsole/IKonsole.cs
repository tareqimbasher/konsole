using KonsoleDotNet.Transcripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonsoleDotNet
{
    /// <summary>
    /// Represents a console that can be read and written to.
    /// </summary>
    public interface IKonsole
    {
        /// <summary>
        /// Gets or sets the current foreground color of this console; the color of the text printed by this console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the current background color of this console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The defaults this console uses.
        /// </summary>
        KonsoleDefaults Defaults { get; }

        /// <summary>
        /// If transcript logging is active, this is the transcript this console is logging to. This property
        /// will be <see langword="null"/> if transcript logging is not active. Use <see cref="StartTranscriptLogging(ITranscript)"/>
        /// to activate transcript logging.
        /// </summary>
        ITranscript Transcript { get; }

        /// <summary>
        /// Writes the specified string to the console with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor);


        /// <summary>
        /// Writes text as info output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole Info(string text);

        /// <summary>
        /// Writes text as debug output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole Debug(string text);

        /// <summary>
        /// Writes text as warning output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole Warn(string text);

        /// <summary>
        /// Writes text as error output.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole Error(string text);

        /// <summary>
        /// Resets this console's colors to their default setting.
        /// </summary>
        IKonsole ResetColors();

        /// <summary>
        /// Starts transcript logging for this console and sets <see cref="Transcript"/> to the specified transcript.
        /// This method can be called multiple times. Each time it is called, it will use the specified transcript
        /// as the new transcript this console will log to.
        /// </summary>
        /// <param name="transcript">The transcript to log to.</param>
        IKonsole StartTranscriptLogging(ITranscript transcript);

        /// <summary>
        /// Stops transcript logging. Also sets <see cref="Transcript"/> to <see langword="null"/>.
        /// </summary>
        /// <returns></returns>
        IKonsole StopTranscriptLogging();
    }
}
