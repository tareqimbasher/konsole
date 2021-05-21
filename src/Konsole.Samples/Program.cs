using KonsoleDotNet.ProgressBars;
using KonsoleDotNet.Transcripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KonsoleDotNet.Samples
{
    class Program
    {
        static void Main()
        {
            IKonsole console = new Konsole();
            console.WriteLine("Starting...");

            var transcript = new Transcript();
            console.StartTranscriptLogging(transcript);

            //var input = console.Write("Enter a num: ").Read(keyInfo =>
            //{
            //    return keyInfo.Key != ConsoleKey.Enter;
            //}, keyInfo =>
            //{
            //    if (keyInfo.KeyChar == 'x')
            //        return "*";
            //    else if (!char.IsDigit(keyInfo.KeyChar) && keyInfo.Key != ConsoleKey.Spacebar)
            //        return "";
            //    else
            //        return keyInfo.KeyChar.ToString();
            //});

            //Console.WriteLine("\nINPUT: " + input);
            //return;

            //console.ReadInput(k =>
            //{
            //    Console.Write(k.KeyChar);

            //    return k.Key != ConsoleKey.Q;
            //});

            BasicUsage(console);
            Prompts(console);
            ProgressBars(console);


            console.StopTranscriptLogging();

            console.WriteLine().WriteDivider().WriteDivider();
            console.WriteLine(transcript.ToString());
        }

        private static void BasicUsage(IKonsole console)
        {
            // Sample 1
            console.WriteLine("Sample 1: The Basics").WriteDivider();
            console.WriteLine("1. Text with default colors.");
            console.WithForeColor(ConsoleColor.Cyan).WriteLine("2. Cyan text on the default background color");
            console.WithBackColor(ConsoleColor.Blue).WriteLine("3. Default text color on a blue background");
            console.WithColors(ConsoleColor.Black, ConsoleColor.Red).WriteLine("4. Black text on a red background");

            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.Yellow;
            console.WriteLine("5. Default text color is now red and default background is now yellow");
            console.WithForeColor(ConsoleColor.Green).WriteLine("6. Green text with, the new default, yellow background");


            // Sample 2
            console.ResetColors().WriteLine("\nSample 2: Ice Cream Menu");
            console
                .WithForeColor(ConsoleColor.Green)
                .WriteDivider()
                .WriteLine("Sunny's Ice Cream Shop")
                .WriteDivider()
                .WithForeColor(ConsoleColor.Cyan).WriteLine("These are your options:")
                .WithForeColor(ConsoleColor.White).WriteLine("   1. Vanilla")
                .WithForeColor(ConsoleColor.DarkMagenta).WriteLine("   2. Chocolate")
                .WithForeColor(ConsoleColor.Red).WriteLine("   3. Strawberry");


            // Sample 3
            console.WriteLine("\nSample 3: Lists").WriteDivider();
            console.WriteLine("Ice Cream Flavors").List("Vanilla", "Chocolate", "Strawberry");
            console.WriteLine("Toppings").OrderedList("Sprinkles", "Strawberries", "Syrup");


            // Sample 4
            console.WriteLine("\nSample 4: Text Alignment").WriteDivider();
            console.WriteLine("This text is left aligned");
            console.WriteLineAlignCenter("This text is center aligned");
            console.WriteLineAlignRight("This text is right aligned");
            console.Write("Left aligned").WriteLineAlignRight("Right aligned");
        }

        private static void Prompts(IKonsole console)
        {
            string name = console.Ask("What is your name?");
            DateTime birthDate = console.Ask<DateTime>("What is your birth date?");
            bool hasPets = console.Confirm("Do you have any pets?");
            int power = console.Ask<int>("What is Goku's power level?");
            var favs = console.Ask("What are your favorite programming languages:", new[] { "C#", "JavaScript", "Python", "COBOL", "Ruby" });

            console
                .WithForeColor(ConsoleColor.Cyan).WriteLine("\nProfile").WriteDivider().ResetColors()
                .WriteLine($"{"Name:",-20} {name}")
                .WriteLine($"{"Age:",-20} {Math.Floor((DateTime.Today - birthDate.Date).TotalDays / 365)}")
                .WriteLine($"{"Pets:",-20} {(hasPets ? "Yes" : "No")}")
                .WriteLine($"{"DBZ Fan?:",-20} {(power > 9000 ? "Yep!" : "No :(")}")
                .WriteLine("Favorite Languages:").List(favs);
        }

        private static void ProgressBars(IKonsole console)
        {
            console.WriteLine("\nProgress Bars").WriteDivider();
            var tasks = new List<Task>();

            var group = console.WithForeColor(ConsoleColor.DarkCyan).ProgressBarGroup();
            
            foreach (var i in Enumerable.Range(1, 5))
            {
                var progressBar = group.ProgressBar($"Async Operation {i}");
                tasks.Add(new Task(() => Work(progressBar, 50 + (i * 50) + (i % 2 * 70))));
            }

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());

            console.WriteLine().Info("Loading is complete!").WriteLine();
        }

        private static void Work(ProgressBar progressBar, int progressDuration)
        {
            var text = progressBar.Text;
            for (int i = 0; i < 10; i++)
            {
                progressBar.Update((i + 1) * 10, $"{text} | Item {i + 1} of 10");
                Thread.Sleep(progressDuration);
            };
        }
    }
}
