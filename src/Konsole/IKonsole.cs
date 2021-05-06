using System;
using System.Collections.Generic;
using System.Text;

namespace KonsoleDotNet
{
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
        /// Writes the specified string to the console with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        IKonsole Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor);

        /// <summary>
        /// Writes the specified string to the console with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        IKonsole Write(string text, ConsoleColor foregroundColor);

        /// <summary>
        /// Writes the specified string to the console with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole Write(string text);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        IKonsole WriteLine(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        IKonsole WriteLine(string text, ConsoleColor foregroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        IKonsole WriteLine(string text);

        /// <summary>
        /// Writes a line terminator to the console.
        /// </summary>
        IKonsole WriteLine();

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
    }
}
