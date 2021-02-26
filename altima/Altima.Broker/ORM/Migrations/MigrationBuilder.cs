using System;
using System.Collections.Generic;
using Altima.Broker.Migrations.Operations;
using Altima.Broker.Relational.Migrations.Operations;
using Altima.Broker.Relational.Migrations.Operations.Builders;

namespace Altima.Broker.Relational.Migrations
{
    public class MigrationBuilder
    {
        private readonly List<MigrationOperation> _operations = new List<MigrationOperation>();
        public virtual List<MigrationOperation> Operations { get => _operations; }

        public virtual CreateTableBuilder CreateTable<TColumns>(
                   string name,
                   Func<ColumnsBuilder, TColumns> columns,
                   string schema = null)
        {
            var createTableOperation = new CreateTableOperation
            {
                Schema = schema,
                Name = name
            };

            var columnsBuilder = new ColumnsBuilder(createTableOperation);
            columns(columnsBuilder);

            Operations.Add(createTableOperation);

            var builder = new CreateTableBuilder(createTableOperation);

            return builder;
        }
    }
}
