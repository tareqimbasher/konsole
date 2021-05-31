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
        /// Ask user for input, returned as a string.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        public static string Ask(this IKonsole konsole, string question) => Ask<string>(konsole, question);

        /// <summary>
        /// Ask user for input and convert input to <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The type to convert user input into.</typeparam>
        /// <param name="question">The text to display to user.</param>
        public static TReturn Ask<TReturn>(this IKonsole konsole, string question)
        {
            var cursorInitTopPosition = Console.CursorTop;

            var converter = TypeDescriptor.GetConverter(typeof(TReturn));
            if (converter == null)
                throw new NotSupportedException($"Type {typeof(TReturn).Name} does not have a valid converter.");

            string input = null;
            while (input == null)
            {
                Console.SetCursorPosition(0, cursorInitTopPosition);
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
        /// Ask user to select from a group of options.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        /// <param name="options">Options the user must select from.</param>
        /// <returns>Selected options.</returns>
        public static IEnumerable<string> Ask(this IKonsole konsole, string question, IEnumerable<string> options)
            => Ask(konsole, question, options, x => x);

        /// <summary>
        /// Ask user to select from a group of options.
        /// </summary>
        /// <param name="question">The text to display to user.</param>
        /// <param name="options">Options the user must select from.</param>
        /// <param name="optionFormatter">A function that returns a string representation of each option.</param>
        /// <returns>Selected options.</returns>
        public static IEnumerable<T> Ask<T>(this IKonsole konsole, string question, IEnumerable<T> options, Func<T, string> optionFormatter)
        {
            if (optionFormatter == null)
                throw new ArgumentNullException(nameof(optionFormatter));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            int cursorInitTopPosition = Console.CursorTop;
            var optionsArr = options.ToArray();

            var selectedOptionIndicies = new HashSet<int>();

            void select(int index)
            {
                if (!selectedOptionIndicies.Contains(index))
                {
                    selectedOptionIndicies.Add(index);
                }
            }

            bool prompt(out string selection)
            {
                selectedOptionIndicies.Clear();

                Console.SetCursorPosition(0, cursorInitTopPosition);
                konsole.WriteLine($"- {question}")
                    .OrderedList(optionsArr.Select(o => optionFormatter(o)))
                    .WriteLine();

                konsole.ClearCurrentLine().Write("[ex: \"1 2 5\" or \"1-4\"]: ");

                selection = konsole.ReadLine();
                if (string.IsNullOrWhiteSpace(selection))
                    return false;

                foreach (var part in selection.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (part.Contains('-'))
                    {
                        var range = part.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        if (range.Length != 2 || 
                            !int.TryParse(range[0], out var start) || 
                            !int.TryParse(range[1], out var end))
                        {
                            return false;
                        }

                        start--;
                        end--;

                        if (start < 0 || (end > optionsArr.Length - 1) || start > end)
                            return false;

                        int rangeCount = start == end ? 1 : (end - start + 1);
                        foreach (var index in Enumerable.Range(start, rangeCount))
                        {
                            select(index);
                        }
                    }
                    else if (int.TryParse(part, out var index))
                    {
                        index--;

                        if (index < 0 || index > optionsArr.Length - 1)
                            return false;

                        select(index);
                    }
                }

                return selectedOptionIndicies.Any();
            }

            while (!prompt(out string selection))
            {
                konsole.ClearCurrentLine()
                    .Error(string.IsNullOrWhiteSpace(selection) ? "Please make a selection" : $"Invalid selection: {selection}")
                    .WriteLine();
            }

            // Clear invalid input message incase it was written
            konsole.ClearCurrentLine();

            return selectedOptionIndicies.Select(ix => optionsArr[ix]);
        }


        /// <summary>
        /// Ask user to yes/no question with no default answer.
        /// </summary>
        /// <param name="text">The text to display to user.</param>
        public static bool Confirm(this IKonsole konsole, string text) => Confirm(konsole, text, null);

        /// <summary>
        /// Ask user to yes/no question with a default answer.
        /// </summary>
        /// <param name="text">The text to display to user.</param>
        /// <param name="defaultAnswer">The default answer if the user hits ENTER without specifying an answer.</param>
        public static bool Confirm(this IKonsole konsole, string text, bool defaultAnswer) => Confirm(konsole, text, defaultAnswer);

        private static bool Confirm(IKonsole konsole, string text, bool? defaultAnswer)
        {
            var cursorInitTopPosition = Console.CursorTop;
            string options;

            if (defaultAnswer == null)
                options = "[y/n]";
            else
                options = defaultAnswer == true ? "[Y/n]" : "[y/N]";

            string selection = null;
            while (selection == null)
            {
                Console.SetCursorPosition(0, cursorInitTopPosition);
                konsole.ClearCurrentLine().Write($"- {text} {options}: ");
                selection = konsole.ReadLine().Trim();

                if (selection.Equals("y", StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (selection.Equals("n", StringComparison.OrdinalIgnoreCase))
                    return false;
                else if (defaultAnswer != null && selection == string.Empty)
                    return defaultAnswer.Value;
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
