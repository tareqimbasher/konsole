using System;
using System.IO;
using System.Text;

namespace KonsoleDotNet
{
    public class MemoryTextWriter : TextWriter
    {
        private readonly StringBuilder _sb = new();

        public string Text => _sb.ToString();

        public string[] Lines => Text.Split(Environment.NewLine);

        public override void Write(char value)
        {
            _sb.Append(value);
        }

        public override Encoding Encoding => Encoding.Default;
    }
}