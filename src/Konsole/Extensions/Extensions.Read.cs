using System;
using System.Text;

namespace KonsoleDotNet
{
    public static class ReadExtensions
    {
        /// <summary>
        /// Pass through to <see cref="Console.Read()"/>.
        /// </summary>
        public static int Read(this IKonsole konsole)
        {
            int input = Console.Read();

            if (input != -1)
            {
                Log(konsole, Convert.ToChar(input).ToString());
            }
            
            return input;
        }

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey()"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this IKonsole konsole)
        {
            var keyInfo = Console.ReadKey();
            Log(konsole, keyInfo.KeyChar.ToString());
            return keyInfo;
        }

        /// <summary>
        /// Pass through to <see cref="Console.ReadKey(bool intercept)"/>.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(this IKonsole konsole, bool intercept)
        {
            var keyInfo = Console.ReadKey(intercept);
            Log(konsole, keyInfo.KeyChar.ToString());
            return keyInfo;
        }

        /// <summary>
        /// Pass through to <see cref="Console.ReadLine()"/>.
        /// </summary>
        public static string ReadLine(this IKonsole konsole)
        {
            var input = Console.ReadLine();
            Log(konsole, input);
            return input;
        }

        /// <summary>
        /// Read user input with the ability to execute an action on each key press and the ability to transform user input.
        /// </summary>
        /// <param name="onKeyDown">A function to execute after each key entered by the user. It should return a <see cref="bool"></see> that indicates
        /// if the method should continue to receive input from the user or should stop and return the collected input.
        /// If <see langword="null"/> is passed, user input will end when the user hits ENTER.</param>
        /// <param name="transform">A function to execute after each key entered by the user that returns a string that the input key should be transformed into.
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

            string str = input.ToString();

            Log(konsole, str);

            return str;
        }

        private static void Log(IKonsole konsole, string text)
        {
            konsole.Transcript?.Add(text, Transcripts.TranscriptLogType.Input, DateTime.UtcNow);
        }
    }
}
