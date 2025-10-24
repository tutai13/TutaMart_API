using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TutaMart_API.Migrations
{
    /// <inheritdoc />
    public partial class create_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    LoaiID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenLoai = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.LoaiID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    SanPhamID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenSanPham = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Gia = table.Column<decimal>(type: "numeric", nullable: false),
                    SoLuongTon = table.Column<int>(type: "integer", nullable: false),
                    HinhAnh = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LoaiID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.SanPhamID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_LoaiID",
                        column: x => x.LoaiID,
                        principalTable: "Categories",
                        principalColumn: "LoaiID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_LoaiID",
                table: "Products",
                column: "LoaiID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
