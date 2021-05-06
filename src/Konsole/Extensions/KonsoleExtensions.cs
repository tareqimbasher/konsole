using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace KonsoleDotNet
{
    public static class KonsoleExtensions
    {
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
            konsole.WriteLine(new string(dividerChar, Console.BufferWidth));
            return konsole;
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an unordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
        public static IKonsole List(this IKonsole konsole, params string[] list)
        {
            return List(konsole, list, bullet: "*");
        }

        /// <summary>
        /// Writes the specified collection of strings to the console in an unordered list format.
        /// </summary>
        /// <param name="list">List to write.</param>
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

        /// <summary>
        /// Pass through to <see cref="Console.Read()"/>.
        /// </summary>
        public static int Read(this IKonsole _) => Console.Read();

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey()"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this IKonsole _) => Console.ReadKey();

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey(bool intercept)"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this IKonsole _, bool intercept) => Console.ReadKey(intercept);

        /// <summary>
        /// Pass through to <see cref="Console.ReadLine()"/>.
        /// </summary>
        public static string ReadLine(this IKonsole _) => Console.ReadLine();


        public static string Ask(this IKonsole konsole, string question) => Ask<string>(konsole, question);

        public static TReturn Ask<TReturn>(this IKonsole konsole, string question)
        {
            var cursorInitialPosition = Console.CursorTop;
            var converter = TypeDescriptor.GetConverter(typeof(TReturn));
            if (converter == null)
                throw new NotSupportedException($"Type {typeof(TReturn).Name} does not have a valid converter.");

            string input = null;
            while (input == null)
            {
                Console.SetCursorPosition(0, cursorInitialPosition);
                konsole.ClearCurrentLine().Write($"- {question} ");
                input = konsole.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    input = null;
                    continue;
                }

                try
                {
                    return (TReturn)converter.ConvertFromInvariantString(input);
                }
                catch
                {
                    input = null;
                }
            }

            return default;
        }

        public static IEnumerable<string> Ask(this IKonsole konsole, string question, IEnumerable<string> options)
            => Ask(konsole, question, options, x => x);

        public static IEnumerable<T> Ask<T>(this IKonsole konsole, string question, IEnumerable<T> options, Func<T, string> optionFormatter)
        {
            int cursorInitialPosition = Console.CursorTop;
            var optionsArr = options.ToArray();
            List<T> selected = new List<T>();

            bool prompt(out string selection)
            {
                selected = new List<T>();
                var added = new HashSet<int>();

                Console.SetCursorPosition(0, cursorInitialPosition);
                konsole.WriteLine($"- {question}")
                    .OrderedList(optionsArr.Select(o => optionFormatter(o)))
                    .WriteLine();

                konsole.ClearCurrentLine().Write("[ex: \"1 2 5\" or \"1-4\"]: ");

                selection = konsole.ReadLine();
                if (string.IsNullOrWhiteSpace(selection))
                    return false;

                foreach (var part in selection.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (part.Contains("-"))
                    {
                        var range = part.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (range.Length != 2)
                            return false;

                        if (!int.TryParse(range[0], out var start))
                            return false;

                        if (!int.TryParse(range[1], out var end))
                            return false;

                        start--;
                        end--;

                        if (start < 0)
                            start = 0;

                        if (end > optionsArr.Length - 1)
                            end = optionsArr.Length - 1;

                        if (start > end)
                            return false;

                        if (start == end && !added.Contains(start))
                        {
                            selected.Add(optionsArr[start]);
                            added.Add(start);
                        }
                        else
                        {
                            var ixRange = Enumerable.Range(start, end - start + 1).ToArray();
                            for (int i = 0; i < ixRange.Length; i++)
                            {
                                int ix = ixRange[i];
                                if (!added.Contains(ix))
                                {
                                    selected.Add(optionsArr[ix]);
                                    added.Add(ix);
                                }
                            }
                        }
                    }
                    else if (int.TryParse(part, out var ix) && !added.Contains(--ix))
                    {
                        if (ix < 0 || ix > optionsArr.Length - 1)
                            return false;

                        selected.Add(optionsArr[ix]);
                        added.Add(ix);
                    }
                }

                return true;
            }

            while (!prompt(out string selection))
            {
                konsole.ClearCurrentLine()
                    .WriteLine(string.IsNullOrWhiteSpace(selection) ? "Please make a selection" : $"Invalid selection: {selection}", ConsoleColor.Red)
                    .WriteLine();
            }

            // Clear invalid input message incase it was written
            konsole.ClearCurrentLine();

            return selected;
        }

        public static bool Confirm(this IKonsole konsole, string text)
        {
            var cursorInitialPosition = Console.CursorTop;

            string selection = null;
            while (selection == null)
            {
                Console.SetCursorPosition(0, cursorInitialPosition);
                konsole.ClearCurrentLine().Write($"- {text} [y/n]: ");
                selection = konsole.ReadLine().Trim();

                if (selection.Equals("y", StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (selection.Equals("n", StringComparison.OrdinalIgnoreCase))
                    return true;
                else
                    selection = null;
            }

            return false;
        }

        public static bool Confirm(this IKonsole konsole, string text, bool defaultAnswer)
        {
            var cursorInitialPosition = Console.CursorTop;
            var options = defaultAnswer ? "[Y/n]" : "[y/N]";

            string selection = null;
            while (selection == null)
            {
                Console.SetCursorPosition(0, cursorInitialPosition);
                konsole.ClearCurrentLine().Write($"- {text} {options}: ");
                selection = konsole.ReadLine().Trim();

                if (selection.Equals("y", StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (selection.Equals("n", StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (selection == string.Empty)
                    return defaultAnswer;
                else
                    selection = null;
            }

            return false;
        }
    }
}
