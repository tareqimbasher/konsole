using System;
using System.Collections.Generic;
using System.Linq;

namespace KonsoleDotNet
{
    public static class OutputExtensions
    {
        /// <summary>
        /// Writes the specified string to the console with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        public static IKonsole Write(this IKonsole konsole, string text, ConsoleColor foregroundColor)
            => konsole.Write(text, foregroundColor, konsole.BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static IKonsole Write(this IKonsole konsole, string text) => konsole.Write(text, konsole.ForegroundColor, konsole.BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color</param>
        public static IKonsole WriteLine(this IKonsole konsole, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            => konsole.Write(text + "\n", foregroundColor, backgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the specified foreground color and the current background color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        public static IKonsole WriteLine(this IKonsole konsole, string text, ConsoleColor foregroundColor)
            => konsole.WriteLine(text, foregroundColor, konsole.BackgroundColor);

        /// <summary>
        /// Writes the specified string to the console followed by a line terminator with the current foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static IKonsole WriteLine(this IKonsole konsole, string text)
            => konsole.WriteLine(text, konsole.ForegroundColor, konsole.BackgroundColor);

        /// <summary>
        /// Writes a line terminator to the console.
        /// </summary>
        public static IKonsole WriteLine(this IKonsole konsole) => konsole.Write("\n");

        /// <summary>
        /// Clears the console.
        /// </summary>
        public static IKonsole Clear(this IKonsole konsole)
        {
            Console.Clear();
            return konsole;
        }

        /// <summary>
        /// Clears the current line and resets the cursor position to the beginning of the line.
        /// </summary>
        public static IKonsole ClearCurrentLine(this IKonsole konsole)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
            return konsole;
        }

        /// <summary>
        /// Replaces the current line with a new value.
        /// </summary>
        public static IKonsole ReplaceCurrentLine(this IKonsole konsole, string text)
        {
            konsole.ClearCurrentLine();
            konsole.Write(text);
            return konsole;
        }

        /// <summary>
        /// Writes a horizontal divider to the console.
        /// </summary>
        /// <param name="dividerChar">The character to use for the divider.</param>
        public static IKonsole WriteDivider(this IKonsole konsole, char dividerChar = '-')
        {
            int length = Console.BufferWidth < 1 ? 1 : Console.BufferWidth;
            konsole.WriteLine(new string(dividerChar, length));
            return konsole;
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an unordered list format.
        /// </summary>
        /// <param name="list">The list to write.</param>
        public static IKonsole List(this IKonsole konsole, params string[] list)
        {
            return List(konsole, list, bullet: "*");
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in a bullet list format.
        /// </summary>
        /// <param name="list">The list to write.</param>
        /// <param name="bullet">A string representing the bullet symbol.</param>
        public static IKonsole List(this IKonsole konsole, IEnumerable<string> list, string bullet = "*")
        {
            string indentation = "   ";
            int numberPadding = list.Count().ToString().Length + 2;
            string newLineReplacement = new string(' ', indentation.Length + numberPadding);

            foreach (var item in list)
            {
                konsole.Write($"{indentation}{bullet} ".PadRight(numberPadding));
                konsole.WriteLine(item?.Replace("\n", $"\n{newLineReplacement}") ?? string.Empty);
            }

            return konsole;
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an ordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        public static IKonsole OrderedList(this IKonsole konsole, params string[] list)
        {
            return OrderedList(konsole, list.AsEnumerable());
        }


        /// <summary>
        /// Writes the specified collection of strings to the console in an ordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        public static IKonsole OrderedList(this IKonsole konsole, IEnumerable<string> list)
        {
            int listCount = list.Count();
            string indentation = "   ";
            int numberPadding = listCount.ToString().Length + 2;
            string newLineReplacement = new string(' ', indentation.Length + numberPadding);

            int count = 0;
            foreach (var item in list)
            {
                konsole.Write($"{indentation}{count + 1}. ".PadRight(numberPadding));
                konsole.WriteLine(item?.Replace("\n", $"\n{newLineReplacement}") ?? string.Empty);
                count++;
            }

            return konsole;
        }

        /// <summary>
        /// Writes the specified value to the console center aligned followed by a line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static IKonsole WriteLineAlignCenter(this IKonsole konsole, string text)
        {
            if (text.Length >= Console.BufferWidth)
            {
                int textLengthToCenter = text.Length % Console.BufferWidth;
                string fullWidth = text.Substring(0, text.Length - textLengthToCenter);
                string textToCenter = text.Substring(text.Length - textLengthToCenter, textLengthToCenter);
                konsole.WriteLine(fullWidth);
                text = textToCenter;
            }

            int paddingLength = (Console.BufferWidth - text.Length) / 2;
            string padding = new string(' ', paddingLength);

            konsole.Write(padding).Write(text).WriteLine(padding);
            return konsole;
        }

        /// <summary>
        /// Writes the specified value to the console right aligned followed by a line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static IKonsole WriteLineAlignRight(this IKonsole konsole, string text)
        {
            konsole.WriteLine(text.PadLeft(Console.BufferWidth - Console.CursorLeft));
            return konsole;
        }
    }
}