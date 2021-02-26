using System.Collections.Generic;
using Altima.Broker.Relational.Migrations;
using Altima.Broker.Relational.Storage;

namespace Altima.Broker.Migrations
{
    public class MigrationCommandListBuilder
    {
        private RelationalCommandBuilder _commandBuilder;
        private readonly List<MigrationCommand> _commands = new List<MigrationCommand>();


        public MigrationCommandListBuilder()
        {
            _commandBuilder = new RelationalCommandBuilder();
        }

        public virtual IReadOnlyList<MigrationCommand> GetCommandList()
        {
            return _commands;
        }

        public virtual MigrationCommandListBuilder EndCommand()
        {
            if (_commandBuilder.CommandTextLength != 0)
            {
                _commands.Add(new MigrationCommand(_commandBuilder.Build()));
                _commandBuilder = new RelationalCommandBuilder();
            }

            return this;
        }

        public virtual MigrationCommandListBuilder Append(string o)
        {
            _commandBuilder.Append(o);

            return this;
        }

        public virtual MigrationCommandListBuilder AppendLine()
        {
            _commandBuilder.AppendLine();

            return this;
        }

        public virtual MigrationCommandListBuilder AppendLine(string value)
        {

            _commandBuilder.AppendLine(value);

            return this;
        }
        public virtual MigrationCommandListBuilder Indent()
        {
            _commandBuilder.Indent();

            return this;
        }


        public virtual MigrationCommandListBuilder IncrementIndent()
        {
            _commandBuilder.IncrementIndent();

            return this;
        }

        public virtual MigrationCommandListBuilder DecrementIndent()
        {
            _commandBuilder.DecrementIndent();

            return this;
        }
    }
}
