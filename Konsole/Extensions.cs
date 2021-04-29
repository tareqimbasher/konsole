using System;
using System.Collections.Generic;
using System.Linq;

namespace Konsole
{
    public static class Extensions
    {
        /// <summary>
        /// Clears the console.
        /// </summary>
        public static Konsole Clear(this Konsole konsole)
        {
            Console.Clear();
            return konsole;
        }

        /// <summary>
        /// Clears the current line and resets the cursor position to the beginning of the line.
        /// </summary>
        public static Konsole ClearCurrentLine(this Konsole konsole)
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
        public static Konsole ReplaceCurrentLine(this Konsole konsole, string text)
        {
            konsole.ClearCurrentLine();
            konsole.Write(text);
            return konsole;
        }

        /// <summary>
        /// Writes a horizontal divider to the console.
        /// </summary>
        /// <param name="dividerChar">The character to use for the divider.</param>
        public static Konsole WriteDivider(this Konsole konsole, char dividerChar = '-')
        {
            konsole.WriteLine(new string(dividerChar, Console.BufferWidth));
            return konsole;
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an unordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        public static Konsole List(this Konsole konsole, params string[] list)
        {
            return List(konsole, list, bullet: "*");
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an unordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        /// <param name="bullet">A string representing the bullet symbol.</param>
        public static Konsole List(this Konsole konsole, IEnumerable<string> list, string bullet = "*")
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
        public static Konsole OrderedList(this Konsole konsole, params string[] list)
        {
            return OrderedList(konsole, list.AsEnumerable());
        }


        /// <summary>
        /// Writes the specified collection of strings to the console in an ordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        public static Konsole OrderedList(this Konsole konsole, IEnumerable<string> list)
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
        public static Konsole WriteLineAlignCenter(this Konsole konsole, string text)
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
        public static Konsole WriteLineAlignRight(this Konsole konsole, string text)
        {
            konsole.WriteLine(text.PadLeft(Console.BufferWidth - Console.CursorLeft));
            return konsole;
        }

        /// <summary>
        /// Pass through to <see cref="Console.Read()"/>.
        /// </summary>
        public static int Read(this Konsole _) => Console.Read();

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey()"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this Konsole _) => Console.ReadKey();

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey(bool intercept)"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this Konsole _, bool intercept) => Console.ReadKey(intercept);

        /// <summary>
        /// Pass through to <see cref="Console.ReadLine()"/>.
        /// </summary>
        public static string ReadLine(this Konsole _) => Console.ReadLine();
    }

    public static class KonsoleScopingExtensions
    {
        internal static KonsoleScope CreateScope(this Konsole konsole)
            => new KonsoleScope(konsole.ForegroundColor, konsole.BackgroundColor);

        /// <summary>
        /// Sets the colors to use within a scope.
        /// </summary>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color.</param>
        public static Konsole WithColors(this Konsole konsole, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (konsole is KonsoleScope pipeline)
            {
                pipeline.ForegroundColor = foregroundColor;
                pipeline.BackgroundColor = backgroundColor;
                return pipeline;
            }
            else
            {
                return new KonsoleScope(foregroundColor, backgroundColor);
            }
        }

        /// <summary>
        /// Sets the foreground color within a scope.
        /// </summary>
        public static Konsole WithForeColor(this Konsole konsole, ConsoleColor foregroundColor)
        {
            if (konsole is KonsoleScope pipeline)
            {
                pipeline.ForegroundColor = foregroundColor;
                return pipeline;
            }
            else
            {
                return new KonsoleScope(foregroundColor, konsole.BackgroundColor);
            }
        }

        /// <summary>
        /// Sets the background color within a scope.
        /// </summary>
        /// <param name="konsole"></param>
        /// <param name="backgroundColor"></param>
        /// <returns></returns>
        public static Konsole WithBackColor(this Konsole konsole, ConsoleColor backgroundColor)
        {
            if (konsole is KonsoleScope pipeline)
            {
                pipeline.BackgroundColor = backgroundColor;
                return pipeline;
            }
            else
            {
                return new KonsoleScope(konsole.ForegroundColor, backgroundColor);
            }
        }
    }
}
