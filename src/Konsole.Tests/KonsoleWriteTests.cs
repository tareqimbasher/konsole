using System;
using Xunit;

namespace KonsoleDotNet
{
    public class KonsoleWriteTests : TestBase
    {
        [Fact]
        public void Write()
        {
            var konsole = new Konsole();
            konsole.Write("Test");
            Assert.Equal("Test", _output.Text);
        }

        [Fact]
        public void WriteLine()
        {
            var konsole = new Konsole();
            konsole.WriteLine("Test");
            Assert.Equal("Test\n", _output.Text);
        }
    }
}