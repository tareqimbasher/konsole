using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;

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
        /// Writes the specified collection of strings to the console in a bullet list format.
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

        /// <summary>
        /// Read user input with the ability to execute an action on each key press and the ability to transform user input.
        /// </summary>
        /// <param name="onKeyDown">A function to execute after each key entered by the user. It should return a <see cref="bool"></see> that indicates
        /// if the method should continue to receive input from the user or should stop and return the collected input.
        /// If <see langword="null"/> is passed, user input will end when the user hits ENTER.</param>
        /// <param name="transform">A function to execute after each key entered by the user that returns a string indicating the character that input key should be transformed into.
        /// If <see langword="null"/> is passed, user input will not be transformed.</param>
        public static string Read(this IKonsole konsole, Func<ConsoleKeyInfo, bool> onKeyDown = null, Func<ConsoleKeyInfo, string> transform = null)
        {
            if (onKeyDown == null)
                onKeyDown = _ => _.Key != ConsoleKey.Enter;

            if (transform == null)
                transform = _ => _.KeyChar.ToString();

            ConsoleKeyInfo keyInfo;
            var input = new StringBuilder();

            do
            {
                keyInfo = konsole.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    konsole.Write("\b \b");
                    input.Remove(input.Length - 1, 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    string c = transform(keyInfo);

                    if (!string.IsNullOrEmpty(c))
                    {
                        konsole.Write(c);
                        input.Append(c);
                    }
                }
            }
            while (onKeyDown(keyInfo));

            return input.ToString();
        }



        /// <summary>
        /// Ask user for input and convert input to <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The type to convert user input into.</typeparam>
        /// <param name="question">The text to display to user.</param>
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

        /// <summary>
        /// Ask user for input, returned as a string.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        public static string Ask(this IKonsole konsole, string question) => Ask<string>(konsole, question);


        /// <summary>
        /// Ask user to select from a group of options.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        /// <param name="options">Options the user must select from.</param>
        public static IEnumerable<string> Ask(this IKonsole konsole, string question, IEnumerable<string> options)
            => Ask(konsole, question, options, x => x);

        /// <summary>
        /// Ask user to select from a group of options.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        /// <param name="options">Options the user must select from.</param>
        /// <param name="optionFormatter">A function that returns a string representation of each option.</param>
        /// <returns></returns>
        public static IEnumerable<T> Ask<T>(this IKonsole konsole, string question, IEnumerable<T> options, Func<T, string> optionFormatter)
        {
            if (optionFormatter == null)
                throw new ArgumentNullException(nameof(optionFormatter));

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


        /// <summary>
        /// Ask user to yes/no question with no default answer.
        /// </summary>
        /// <param name="text">The text to display to user.</param>
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

        /// <summary>
        /// Ask user to yes/no question with a default answer.
        /// </summary>
        /// <param name="text">The text to display to user.</param>
        /// <param name="defaultAnswer">The default answer if the user hits ENTER without specifying an answer.</param>
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



        /// <summary>
        /// Ask user for password input.
        /// </summary>
        /// <param name="showMask">If <see langword="true"/>, will show a '*' for each character the user inputs. If <see langword="false"/>, user input is hidden.</param>
        public static string Password(this IKonsole konsole, bool showMask = false)
        {
            StringBuilder password = new StringBuilder();

            Password(konsole, showMask, c => password.Append(c), () => password = password.Remove(password.Length - 1, 1));

            return password.ToString();
        }

        /// <summary>
        /// Ask user for password input and return the password as a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="showMask">If <see langword="true"/>, will show a '*' for each character the user inputs. If <see langword="false"/>, user input is hidden.</param>
        /// <returns></returns>
        public static SecureString PasswordAsSecureString(this IKonsole konsole, bool showMask = false)
        {
            SecureString password = new SecureString();

            Password(konsole, showMask, c => password.AppendChar(c), () => password.RemoveAt(password.Length - 1));

            return password;
        }

        private static void Password(IKonsole konsole, bool showMask, Action<char> appendChar, Action removeChar)
        {
            ConsoleKeyInfo keyInfo;
            int passwordLength = 0;

            do
            {
                keyInfo = konsole.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Backspace && passwordLength > 0)
                {
                    if (showMask)
                        konsole.Write("\b \b");

                    removeChar();
                    passwordLength--;
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    if (showMask)
                        konsole.Write("*");

                    appendChar(keyInfo.KeyChar);
                    passwordLength++;
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
        }
    }
}
