using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;

namespace KonsoleDotNet
{
    public static class PromptExtensions
    {
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
