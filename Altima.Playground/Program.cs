using Altima.Broker.Business.Types;
using Altima.Broker.Relational.Metadata;
using Altima.Broker.Relational.Migrations.Internal;
using System;
using System.Collections.Generic;

namespace Altima.Playground
{
    class Matricula: StringType
    {
        private Matricula(string value) => Value = value;
        public static implicit operator Matricula(string value) => new Matricula(value);
    }

    class Cooperado
    {
        public Matricula Matricula { get; set; }
    }

    class Program
    {
       static void Main(string[] args)
        {
            var cooperado = new Cooperado();
            cooperado.Matricula = "100";

            Console.WriteLine(cooperado.Matricula);
            Console.ReadKey();

            var columns = new List<Column>();
            columns.Add(new Column("codigo", "integer", 0, 0, 0, false));
            columns.Add(new Column("codigo", "varchar", 200, 0, 0, true));


            var table = new Table("dbo","Cooperado", columns);

            var tables = new List<Table>();
            tables.Add(table);


            var migrator = new Migrator(tables);

            Console.WriteLine(migrator.GenerateScript());
            Console.ReadKey();
        }
    }
}
