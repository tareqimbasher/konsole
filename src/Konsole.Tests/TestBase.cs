using System;
using Xunit;

namespace KonsoleDotNet
{
    [Collection("Sequential")]
    public class TestBase : IDisposable
    {
        protected readonly MemoryTextWriter _output;

        public TestBase()
        {
            _output = new MemoryTextWriter();
            Console.SetOut(_output);
        }
        
        public void Dispose()
        {
            _output.Dispose();
        }
    }
}