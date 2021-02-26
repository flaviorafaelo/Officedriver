using System;
using System.Collections.Generic;
using System.Linq;
using Altima.Broker.Migrations;
using Altima.Broker.Relational.Metadata;
using Altima.Broker.Relational.Metadata.Internal;
using Altima.Broker.Relational.Migrations.Operations;

namespace Altima.Broker.Relational.Migrations.Internal
{
    public class Migrator
    {
        private readonly MigrationsRelationalDiffer _differ;
        public Migrator(List<Table> tables) 
        {
            RelationalModel source = new RelationalModel(tables);
            RelationalModel target = new RelationalModel(new List<Table>());
            _differ = new MigrationsRelationalDiffer(source, target);
        }

        public virtual string GenerateScript()
        {
            var operations = _differ.GetDifferences();
            var sqlGenerator = new MigrationsSqlGenerator();
            var commandList = sqlGenerator.Generate(operations);
            return string.Concat(commandList.Select(c => c.CommandText));
        }

        private List<MigrationOperation> BuildOperations(Action<MigrationBuilder> buildAction)
        {
            var migrationBuilder = new MigrationBuilder();
            buildAction(migrationBuilder);

            return migrationBuilder.Operations;
        }
    }
}
