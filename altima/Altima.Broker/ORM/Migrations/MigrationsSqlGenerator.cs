using System;
using System.Collections.Generic;
using Altima.Broker.Migrations.Operations;
using Altima.Broker.Relational.Metadata;
using Altima.Broker.Relational.Migrations;
using Altima.Broker.Relational.Migrations.Operations;
using Altima.Broker.Relational.Storage;
using Altima.Broker.Relational.Storage.Internal;

namespace Altima.Broker.Migrations
{
    public class MigrationsSqlGenerator
    {
        private SqlServerSqlGenerationHelper _sqlGenerationHelper = new SqlServerSqlGenerationHelper();

        private static readonly
                   IReadOnlyDictionary<Type, Action<MigrationsSqlGenerator, MigrationOperation,  MigrationCommandListBuilder>>
                   _generateActions =
                       new Dictionary<Type, Action<MigrationsSqlGenerator, MigrationOperation, MigrationCommandListBuilder>>
                       {
                    { typeof(AddColumnOperation), (g, o, b) => g.Generate((AddColumnOperation)o, b) },
                   // { typeof(AddForeignKeyOperation), (g, o, m, b) => g.Generate((AddForeignKeyOperation)o, m, b) },
                    { typeof(AddPrimaryKeyOperation), (g, o, b) => g.Generate((AddPrimaryKeyOperation)o,  b) },
                   /* { typeof(AddUniqueConstraintOperation), (g, o, m, b) => g.Generate((AddUniqueConstraintOperation)o, m, b) },
                    { typeof(AlterColumnOperation), (g, o, m, b) => g.Generate((AlterColumnOperation)o, m, b) },
                    { typeof(AlterDatabaseOperation), (g, o, m, b) => g.Generate((AlterDatabaseOperation)o, m, b) },
                    { typeof(AlterSequenceOperation), (g, o, m, b) => g.Generate((AlterSequenceOperation)o, m, b) },
                    { typeof(AlterTableOperation), (g, o, m, b) => g.Generate((AlterTableOperation)o, m, b) },
                    { typeof(AddCheckConstraintOperation), (g, o, m, b) => g.Generate((AddCheckConstraintOperation)o, m, b) },
                    { typeof(CreateIndexOperation), (g, o, m, b) => g.Generate((CreateIndexOperation)o, m, b) },
                    { typeof(CreateSequenceOperation), (g, o, m, b) => g.Generate((CreateSequenceOperation)o, m, b) },*/
                    { typeof(CreateTableOperation), (g, o, b) => g.Generate((CreateTableOperation)o, b) },
                    /*{ typeof(DropColumnOperation), (g, o, m, b) => g.Generate((DropColumnOperation)o, m, b) },
                    { typeof(DropForeignKeyOperation), (g, o, m, b) => g.Generate((DropForeignKeyOperation)o, m, b) },
                    { typeof(DropIndexOperation), (g, o, m, b) => g.Generate((DropIndexOperation)o, m, b) },
                    { typeof(DropPrimaryKeyOperation), (g, o, m, b) => g.Generate((DropPrimaryKeyOperation)o, m, b) },
                    { typeof(DropSchemaOperation), (g, o, m, b) => g.Generate((DropSchemaOperation)o, m, b) },
                    { typeof(DropSequenceOperation), (g, o, m, b) => g.Generate((DropSequenceOperation)o, m, b) },
                    { typeof(DropTableOperation), (g, o, m, b) => g.Generate((DropTableOperation)o, m, b) },
                    { typeof(DropUniqueConstraintOperation), (g, o, m, b) => g.Generate((DropUniqueConstraintOperation)o, m, b) },
                    { typeof(DropCheckConstraintOperation), (g, o, m, b) => g.Generate((DropCheckConstraintOperation)o, m, b) },
                    { typeof(EnsureSchemaOperation), (g, o, m, b) => g.Generate((EnsureSchemaOperation)o, m, b) },
                    { typeof(RenameColumnOperation), (g, o, m, b) => g.Generate((RenameColumnOperation)o, m, b) },
                    { typeof(RenameIndexOperation), (g, o, m, b) => g.Generate((RenameIndexOperation)o, m, b) },
                    { typeof(RenameSequenceOperation), (g, o, m, b) => g.Generate((RenameSequenceOperation)o, m, b) },
                    { typeof(RenameTableOperation), (g, o, m, b) => g.Generate((RenameTableOperation)o, m, b) },
                    { typeof(RestartSequenceOperation), (g, o, m, b) => g.Generate((RestartSequenceOperation)o, m, b) },
                    { typeof(SqlOperation), (g, o, m, b) => g.Generate((SqlOperation)o, m, b) },
                    { typeof(InsertDataOperation), (g, o, m, b) => g.Generate((InsertDataOperation)o, m, b) },
                    { typeof(DeleteDataOperation), (g, o, m, b) => g.Generate((DeleteDataOperation)o, m, b) },
                    { typeof(UpdateDataOperation), (g, o, m, b) => g.Generate((UpdateDataOperation)o, m, b) }*/
                       };

        public virtual IReadOnlyList<MigrationCommand> Generate(
            IReadOnlyList<MigrationOperation> operations)
        {
            var builder = new MigrationCommandListBuilder();
            
            foreach (var operation in operations)
            {
                Generate(operation, builder);
            }

            return builder.GetCommandList();
        }

        protected virtual void EndStatement(
            MigrationCommandListBuilder builder)
        {
            builder.EndCommand();
        }

        protected virtual void Generate(
                    MigrationOperation operation,
                    MigrationCommandListBuilder builder)
        {
            var operationType = operation.GetType();
            if (!_generateActions.TryGetValue(operationType, out var generateAction))
            {
                throw new ArgumentException("Aqui");// InvalidOperationException(RelationalStrings.UnknownOperation(GetType().ShortDisplayName(), operationType));
            }

            generateAction(this, operation, builder);
        }

        protected virtual void Generate(
            CreateTableOperation operation,
            MigrationCommandListBuilder builder)
        {
            builder
                .Append("CREATE TABLE ")
                .Append(operation.Name)
                .AppendLine(" (")
                .Indent();

             CreateTableColumns(operation, builder);

             //CreateTableConstraints(operation, model, builder);
            
            builder.AppendLine()
                   .Append(")");

            EndStatement(builder);
        }

        protected virtual void CreateTableColumns(
                    CreateTableOperation operation,
                    MigrationCommandListBuilder builder)
        {
            for (var i = 0; i < operation.Columns.Count; i++)
            {
                ColumnDefinition(operation.Columns[i], builder);

                if (i != operation.Columns.Count - 1)
                {
                    builder.AppendLine(",");
                }
            }
        }

        protected virtual void ColumnDefinition(
                    ColumnOperation operation,
                    MigrationCommandListBuilder builder)
        {
            var columnType = operation.ColumnType;
            builder
                .Append(_sqlGenerationHelper.DelimitIdentifier(operation.Name))
                .Append(" ")
                .Append(columnType);

            builder.Append(operation.IsNullable ? " NULL" : " NOT NULL");

            //DefaultValue(operation.DefaultValue, operation.DefaultValueSql, columnType, builder);
        }

    }
}
