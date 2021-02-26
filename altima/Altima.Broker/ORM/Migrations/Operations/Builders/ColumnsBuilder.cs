using Altima.Broker.Migrations.Operations;

namespace Altima.Broker.Relational.Migrations.Operations.Builders
{
    public class ColumnsBuilder
    {
        private readonly CreateTableOperation _createTableOperation;

        public ColumnsBuilder(CreateTableOperation createTableOperation)
        {
            _createTableOperation = createTableOperation;
        }

        public virtual OperationBuilder<AddColumnOperation> Column<T>(
                    string type = null,
                    //bool? unicode = null,
                    int? maxLength = null,
                   // bool rowVersion = false,
                    string name = null,
                    bool nullable = false,
                    object defaultValue = null,
                    //string defaultValueSql = null,
                   // string computedColumnSql = null,
                    //bool? fixedLength = null,
                    string comment = null,
                    //string collation = null,
                    int? precision = null,
                    int? scale = null
                    //bool? stored = null
                    )
        {
            var operation = new AddColumnOperation
            {
                Schema = _createTableOperation.Schema,
                Table = _createTableOperation.Name,
                Name = name,
                //ClrType = typeof(T),
                ColumnType = type,
                //IsUnicode = unicode,
                MaxLength = maxLength,
                //IsRowVersion = rowVersion,
                IsNullable = nullable,
                DefaultValue = defaultValue,
                //DefaultValueSql = defaultValueSql,
                //ComputedColumnSql = computedColumnSql,
                //IsFixedLength = fixedLength,
                Comment = comment,
                //Collation = collation,
                Precision = precision,
                Scale = scale,
                //IsStored = stored
            };
            _createTableOperation.Columns.Add(operation);

            return new OperationBuilder<AddColumnOperation>(operation);
        }
    }
}
