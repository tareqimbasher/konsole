using System.Linq;
using Xunit;

namespace KonsoleDotNet
{
    public class KonsoleOutputExtensionTests : TestBase
    {
        [Fact]
        public void WriteDivider()
        {
            var konsole = new Konsole();
            konsole.WriteDivider();
            var output = _output.Text;
            var lastChar = output.Last();
            var rest = output.Substring(0, output.Length - 2);

            Assert.Equal('\n', lastChar);
            Assert.All(rest, c => Assert.Equal('-', c));
        }

        [Fact]
        public void List()
        {
            var konsole = new Konsole();
            var list = new[] { "One", "Two", "Three" };

            konsole.List(list);

            Assert.Equal(list.Select(i => $"   * {i}").Union(new[] { "" }), _output.Lines);
        }
        
        [Fact]
        public void List_With_Specific_Bullet()
        {
            var konsole = new Konsole();
            var list = new[] { "One", "Two", "Three" };

            konsole.List(list, "--");

            Assert.Equal(list.Select(i => $"   -- {i}").Union(new[] { "" }), _output.Lines);
        }
        
        [Fact]
        public void OrdererdList()
        {
            var konsole = new Konsole();
            var list = new[] { "One", "Two", "Three" };

            konsole.OrderedList(list);

            int counter = 0;
            Assert.Equal(list.Select(i => $"   {++counter}. {i}").Union(new[] { "" }), _output.Lines);
        }
    }
}