using System.IO;
using System.Text;

namespace Altima.Broker.Relational.Infrastructure
{
    public class IndentedStringBuilder
    {
        private const byte IndentSize = 4;
        private byte _indent;
        private bool _indentPending = true;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public virtual int Length => _stringBuilder.Length;

        public virtual IndentedStringBuilder Append(string value)
        {
            DoIndent();

            _stringBuilder.Append(value);

            return this;
        }

        public virtual IndentedStringBuilder AppendLine(string value)
        {
            if (value.Length != 0)
            {
                DoIndent();
            }

            _stringBuilder.AppendLine(value);

            _indentPending = true;

            return this;
        }

        public virtual IndentedStringBuilder AppendLine()
        {
            AppendLine(string.Empty);

            return this;
        }

        private void DoIndent()
        {
            if (_indentPending && _indent > 0)
            {
                _stringBuilder.Append(new string(' ', _indent * IndentSize));
            }

            _indentPending = false;
        }

        public virtual IndentedStringBuilder IncrementIndent()
        {
            _indent++;

            return this;
        }

        public virtual IndentedStringBuilder DecrementIndent()
        {
            if (_indent > 0)
            {
                _indent--;
            }

            return this;
        }

     
        public virtual IndentedStringBuilder Clear()
        {
            _stringBuilder.Clear();
            _indent = 0;

            return this;
        }

        public virtual IndentedStringBuilder AppendLines(string value, bool skipFinalNewline = false)
        {
            using (var reader = new StringReader(value))
            {
                var first = true;
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        AppendLine();
                    }

                    if (line.Length != 0)
                    {
                        Append(line);
                    }
                }
            }

            if (!skipFinalNewline)
            {
                AppendLine();
            }

            return this;
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}
