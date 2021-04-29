using System;

namespace Konsole.Samples
{
    class Program
    {
        static void Main()
        {
            var console = new Konsole();

            // Sample 1
            console.WriteLine("Sample 1: The Basics").WriteDivider();
            console.WriteLine("1. Text with default colors.");
            console.WithForeColor(ConsoleColor.Cyan).WriteLine("2. Cyan text on the default background");
            console.WithBackColor(ConsoleColor.Blue).WriteLine("3. Default text on a blue background");
            console.WithForeColor(ConsoleColor.Black).WithBackColor(ConsoleColor.Red).WriteLine("4. Black text on a red background");

            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.Yellow;
            console.WriteLine("5. Default text color is now red and default background is now yellow");
            console.WithForeColor(ConsoleColor.Green).WriteLine("6. Green text with (the new default) yellow background");


            // Sample 2
            console.WriteLine().ResetColors().WriteLine("Sample 2: Ice Cream Menu");
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
            console.WriteLine().WriteLine("Sample 3: Lists").WriteDivider();
            console.WriteLine("Ice Cream Flavors").List("Vanilla", "Chocolate", "Strawberry");
            console.WriteLine("Toppings").OrderedList("Sprinkles", "Strawberries", "Syrup");


            // Sample 4
            console.WriteLine().WriteLine("Sample 4: Text Alignment").WriteDivider();
            console.WriteLine("This text is left aligned");
            console.WriteLineAlignCenter("This text is center aligned");
            console.WriteLineAlignRight("This text is right aligned");
            console.Write("Left aligned").WriteLineAlignRight("Right aligned");
        }
    }
}
