using Altima.Broker.ORM.Storage;
using Altima.Broker.Relational.Infrastructure;
using System;

namespace Altima.Broker.Relational.Storage
{
    public class RelationalCommandBuilder
    {
        private readonly IndentedStringBuilder _commandTextBuilder = new IndentedStringBuilder();

        public virtual RelationalCommandBuilder Append(string value)
        {

            _commandTextBuilder.Append(value);

            return this;
        }

        public virtual RelationalCommandBuilder IncrementIndent()
        {
            _commandTextBuilder.IncrementIndent();

            return this;
        }

        public virtual RelationalCommandBuilder DecrementIndent()
        {
            _commandTextBuilder.DecrementIndent();

            return this;
        }

        public virtual RelationalCommandBuilder AppendLine()
        {
            _commandTextBuilder.AppendLine();

            return this;
        }

        public virtual int CommandTextLength => _commandTextBuilder.Length;

        public virtual RelationalCommandBuilder AppendLine(string value)
        {
            _commandTextBuilder.Append(value).AppendLine();

            return this;
        }

        public virtual RelationalCommandBuilder Indent()
        {
            _commandTextBuilder.IncrementIndent();

            return this;
        }

        public virtual RelationalCommand Build()
        {
            return new RelationalCommand(_commandTextBuilder.ToString()/*, Parameters*/);
        }
    }
}
