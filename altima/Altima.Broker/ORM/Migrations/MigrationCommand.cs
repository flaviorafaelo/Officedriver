
using Altima.Broker.ORM.Storage;

namespace Altima.Broker.Relational.Migrations
{
    public class MigrationCommand
    {
        public MigrationCommand(RelationalCommand relationalCommand
                                //DbContext context,
                                //IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger
                                )
        {
            _relationalCommand = relationalCommand;
        }
        private readonly RelationalCommand _relationalCommand;

        public virtual string CommandText => _relationalCommand.CommandText;

        //public virtual int ExecuteNonQuery(
        //    [NotNull] IRelationalConnection connection,
        //    [CanBeNull] IReadOnlyDictionary<string, object> parameterValues = null)
        //    => _relationalCommand.ExecuteNonQuery(
        //        new RelationalCommandParameterObject(
        //            connection,
        //            parameterValues,
        //            null,
        //            _context,
        //            CommandLogger));
    }
}
