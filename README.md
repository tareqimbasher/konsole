# <img align="left" width="48" height="48" src="https://github.com/tareqimbasher/konsole/blob/main/docs/icon-128.png?raw=true"> Konsole

[![dev](https://github.com/tareqimbasher/konsole/actions/workflows/dev.yml/badge.svg)](https://github.com/tareqimbasher/konsole/actions/workflows/dev.yml)

A light-weight System.Console utility wrapper for .NET.

## Install

Package Manager

```
Install-Package Konsole
```

.NET CLI

```
dotnet add package Konsole
```


## Why?
When writing applications geared for use with the console, we often end up rolling our own 
helpers for formatting output, getting user input..etc. This library aims to make that job just a little bit easier.


## Usage

You can use the built-in `Konsole` class to quickly get up and running:

```csharp
IKonsole console = new Konsole();
```

You can also created your own implementation of the `IKonsole` interface, or inherit from `Konsole` and override 
the class members you need:

```csharp
public class MyConsole : Konsole
{
    public override IKonsole Write(string text)
    {
        return base.Write($"My app: {text}");
    }
}
```

Use it like you would `System.Console`

```csharp
console.WriteLine("Lorem ipsum");
```


#### Defaults

You can change the defaults for an `IKonsole` instance. Below are the defaults out of the box:

```csharp
console.Defaults.ForegroundColor = Console.ForegroundColor;
console.Defaults.BackgroundColor = Console.BackgroundColor;

console.Defaults.Info = (console, text) => console.WriteLine(text, ConsoleColor.White);
console.Defaults.Debug = (console, text) => console.WriteLine(text, ConsoleColor.Green);
console.Defaults.Warn = (console, text) => console.WriteLine(text, ConsoleColor.Yellow);
console.Defaults.Error = (console, text) => console.WriteLine(text, ConsoleColor.Red);

console.Defaults.PostWriteAction = (console, text) => { }; // Gets called after every write to the console
```

#### Chaining
All methods are chainable.

```csharp
console.WriteLine("Header")
    .WriteDivider()
    .Write("Status: ").WriteLine("Loaded");
```


#### Coloring Output

`Write()` and `WriteLine()` have overrides to specify foreground and background colors.

```csharp
console.Write("Status: ", ConsoleColor.Green).WriteLine("Loaded");
```

You can set the current foreground and background colors.

```csharp
console.ForegroundColor = ConsoleColor.Green;
console.BackgroundColor = ConsoleColor.Black;
```

Use `ResetColors()` to reset the current foreground and background colors as defined in `console.Defaults`

```csharp
console.ResetColors();
```

#### Formatting Output

```csharp
console.Clear();
console.ClearCurrentLine();
console.ReplaceCurrentLine("New text");
console.WriteDivider();
console.List("Vanilla", "Chocolate", "Strawberry");
console.OrderedList("Vanilla", "Chocolate", "Strawberry");
console.WriteLineAlignCenter("Center aligned text");
console.WriteLineAlignRight("Right aligned text");
console.Debug("Debug message");
console.Info("Info message");
console.Warn("Warning message");
console.Error("Error message");
```


#### Scoping
Use `WithForeColor()`, `WithBackColor()`, and `WithColors()` to apply colors within a scope without changing the 
default colors of the `IKonsole` instance you're using.

```csharp
console.WriteLine("Default color text")
    .WithForeColor(ConsoleColor.Green).WriteLine("Green text");

console.WriteLine("Still uses default color text");
```


#### Prompts

Use the `Ask()` method to get input from the user. Use the `Ask<T>` overload to convert user input into the specified 
data type.

```csharp
string name = console.Ask("What is your name?");
DateTime birthDate = console.Ask<DateTime>("What is your birth date?");
int power = console.Ask<int>("What is Goku's power level?");
var favs = console.Ask("What are your favorite programming languages:",
    new[] { "C#", "JavaScript", "Python", "COBOL", "Ruby" });
```

> :bulb: Create a custom `TypeConverter` to convert user input to a non-primitive data type. 
Internally, this method uses `TypeConverter.ConvertFromInvariantString(input)` to do the conversion.

Use the `Confirm()` method to get a yes/no answer from the user.

```csharp
bool hasPets = console.Confirm("Do you have any pets?");
bool proceed = console.Confirm("Do you want to continue?", defaultAnswer: false);
```

#### Progress Bars

A single progress bar

```csharp
var progressBar = console.ProgressBar("Loading file");
progressBar.Update(30); // Set progress to 30%
```

Multiple progress bars can working simultaneously when added to a group

```csharp
var group = console.ProgressBarGroup();
var progressBar1 = group.ProgressBar("File 1");
var progressBar2 = group.ProgressBar("File 2");

progressBar1.Update(50);
progressBar2.Update(35);
```

<br/>

## Code Samples

Checkout the `Konsole.Samples` project for a working example.

#### Output Formatting

```csharp
IKonsole console = new Konsole();

// Sample 1
console.WriteLine("Sample 1: The Basics").WriteDivider();
console.WriteLine("1. Text with default colors.");
console.WithForeColor(ConsoleColor.Cyan).WriteLine("2. Cyan text on the default background");
console.WithBackColor(ConsoleColor.Blue).WriteLine("3. Default text on a blue background");
console.WithForeColor(ConsoleColor.Black).WithBackColor(ConsoleColor.Red).WriteLine("4. Black text on a red background");

console.ForegroundColor = ConsoleColor.Red;
console.BackgroundColor = ConsoleColor.Yellow;

console.WriteLine("5. Text color is now red and background is now yellow");
console.WithForeColor(ConsoleColor.Green).WriteLine("6. Green text with a yellow background");


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
```

This results in the following output:

![preview-output-formatting](https://github.com/tareqimbasher/konsole/blob/main/docs/preview-output-formatting.png?raw=true)

#### Prompts

```csharp
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
```

This results in the following output:

![preview-prompts](https://github.com/tareqimbasher/konsole/blob/main/docs/preview-prompts.png?raw=true)


#### Progress Bars

##### Single Progress Bar

```csharp
var progressBar = console.ProgressBar("Loading file");

for (int i = 1; i <= 10; i++)
{
    progressBar.Update(i * 10);
}

console.Info("Loading complete.");
```

##### Progress Bar Group
Multiple progress bars running asynchronously.

```csharp
private void Process()
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


private void Work(ProgressBar progressBar, int progressDuration)
{
    var text = progressBar.Text;
    for (int i = 0; i < 10; i++)
    {
        progressBar.Update((i + 1) * 10, $"{text} | Item {i + 1} of 10");
        Thread.Sleep(progressDuration);
    };
}

```

This results in the following output:

![preview-progressbars](https://github.com/tareqimbasher/konsole/blob/main/docs/preview-progressbars.gif?raw=true)
