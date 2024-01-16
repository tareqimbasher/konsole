using System;
using System.Collections.Generic;
using System.Linq;

namespace KonsoleDotNet
{
    public static partial class OutputExtensions
    {
        /// <summary>
        /// Writes a line terminator to the console.
        /// </summary>
        public static IKonsole WriteLine(this IKonsole konsole) => konsole.Write("\n");

        /// <summary>
        /// Writes the specified string to the console and appends a line terminator.
        /// </summary>
        public static IKonsole WriteLine(this IKonsole konsole, string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            konsole.Write(text, foregroundColor, backgroundColor);
            konsole.Write("\n");
            return konsole;
        }

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
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth - 1));
            Console.CursorLeft = 0;
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
        /// <param name="indent">Indents each list using this many spaces.</param>
        public static IKonsole List(this IKonsole konsole, IEnumerable<string> list, string bullet = "*", int indent = 0)
        {
            string indentation = indent > 0 ? new string(' ', 0) : string.Empty;
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
        /// <param name="indent">Indents each list using this many spaces.</param>
        public static IKonsole OrderedList(this IKonsole konsole, IEnumerable<string> list, int indent = 0)
        {
            int listCount = list.Count();
            string indentation = indent > 0 ? new string(' ', 0) : string.Empty;
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