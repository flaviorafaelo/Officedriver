using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Altima.Broker.AspNet.Mvc.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cooperado",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricula = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RgNumero = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RgExpedicao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    CnhNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CnhValidade = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnderecoCep = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    EnderecoLogradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EnderecoNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EnderecoComplemento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EnderecoBairro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EnderecoEstado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EnderecoCidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContaAgencia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContaNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataMatricula = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDesligamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    TelefoneDdd = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TelefoneNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CelularDdd = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CelularNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooperado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rule = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CooperadoContatos",
                columns: table => new
                {
                    CooperadoId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TelefoneDdd = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TelefoneNumero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CooperadoContatos", x => new { x.CooperadoId, x.Id });
                    table.ForeignKey(
                        name: "FK_CooperadoContatos_Cooperado_CooperadoId",
                        column: x => x.CooperadoId,
                        principalTable: "Cooperado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CooperadoContatos");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Cooperado");
        }
    }
}
