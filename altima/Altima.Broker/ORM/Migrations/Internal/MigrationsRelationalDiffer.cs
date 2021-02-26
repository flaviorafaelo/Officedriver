using System.Collections.Generic;
using Altima.Broker.Relational.Metadata;
using Altima.Broker.Relational.Metadata.Internal;
using Altima.Broker.Relational.Migrations.Operations;

namespace Altima.Broker.Relational.Migrations.Internal
{
    public class MigrationsRelationalDiffer
    {
        private readonly RelationalModel _source;
        private readonly RelationalModel _target;

        public MigrationsRelationalDiffer(RelationalModel source, RelationalModel target)
        {
            _source = source;
            _target = target;
        }

        protected virtual IReadOnlyList<MigrationOperation> Diff()
        {
            var builder = new MigrationBuilder();

            foreach(var t in _source.Tables)
            {

                builder.CreateTable(
                    name: t.Name,
                    columns: table => new
                    {
                        Teste = table.Column<int>(nullable: false),
                        Teste2 = table.Column<int>(nullable: false),
                    });
            }

            return builder.Operations;
        }

        public virtual IReadOnlyList<MigrationOperation> GetDifferences()
        {
            return Diff();
            //var diffContext = new DiffContext();
            //return diffContext;
            // return Sort(Diff(source, target, diffContext), diffContext);
        }
    }
}
