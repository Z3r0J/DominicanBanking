using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DominicanBanking.Infrastructure.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DominicanBanking");

            migrationBuilder.CreateTable(
                name: "Advances",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditCardNumberFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IdentifyNumberTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiary",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentifyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifyNumberFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IdentifyNumberTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_Payments",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Products",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Limit = table.Column<double>(type: "float", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Products_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "DominicanBanking",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "DominicanBanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifyNumberFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IdentifyNumberTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Type_Payments_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "DominicanBanking",
                        principalTable: "Type_Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "DominicanBanking",
                table: "Products",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 7, 13, 16, 43, 8, 255, DateTimeKind.Local).AddTicks(3378), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Savings Account" },
                    { 2, new DateTime(2022, 7, 13, 16, 43, 8, 288, DateTimeKind.Local).AddTicks(5239), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Credit Card" },
                    { 3, new DateTime(2022, 7, 13, 16, 43, 8, 288, DateTimeKind.Local).AddTicks(5276), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Loan" }
                });

            migrationBuilder.InsertData(
                schema: "DominicanBanking",
                table: "Type_Payments",
                columns: new[] { "Id", "Created", "CreatedBy", "Modified", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 7, 13, 16, 43, 8, 290, DateTimeKind.Local).AddTicks(1460), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Express Payment" },
                    { 2, new DateTime(2022, 7, 13, 16, 43, 8, 290, DateTimeKind.Local).AddTicks(1483), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Credit Card Payment" },
                    { 3, new DateTime(2022, 7, 13, 16, 43, 8, 290, DateTimeKind.Local).AddTicks(1485), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Loan Payment" },
                    { 4, new DateTime(2022, 7, 13, 16, 43, 8, 290, DateTimeKind.Local).AddTicks(1488), "DefaultDatabaseAdministrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Beneficiary Payment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TypeId",
                schema: "DominicanBanking",
                table: "Payments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Products_ProductId",
                schema: "DominicanBanking",
                table: "User_Products",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advances",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "Beneficiary",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "User_Products",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "Type_Payments",
                schema: "DominicanBanking");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "DominicanBanking");
        }
    }
}
