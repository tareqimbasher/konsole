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

        [Fact]
        public void Info()
        {
            var konsole = new Konsole();
            konsole.Info("Test");
            Assert.Equal("Test\n", _output.Text);
        }

        [Fact]
        public void Debug()
        {
            var konsole = new Konsole();
            konsole.Debug("Test");
            Assert.Equal("Test\n", _output.Text);
        }

        [Fact]
        public void Warn()
        {
            var konsole = new Konsole();
            konsole.Warn("Test");
            Assert.Equal("Test\n", _output.Text);
        }

        [Fact]
        public void Error()
        {
            var konsole = new Konsole();
            konsole.Error("Test");
            Assert.Equal("Test\n", _output.Text);
        }
    }
}