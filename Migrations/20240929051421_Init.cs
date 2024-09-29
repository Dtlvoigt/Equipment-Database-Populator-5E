using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentDatabasePopulator5E.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MagicItem = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    WeaponCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeNormal = table.Column<int>(type: "int", nullable: true),
                    RangeLong = table.Column<int>(type: "int", nullable: true),
                    ThrowRangeNormal = table.Column<int>(type: "int", nullable: true),
                    ThrowRangeLong = table.Column<int>(type: "int", nullable: true),
                    DamageDice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DamageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoHandedDamageDice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoHandedDamageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialAttribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArmorCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArmorClass = table.Column<int>(type: "int", nullable: true),
                    DexBonus = table.Column<bool>(type: "bit", nullable: true),
                    MaxDexBonus = table.Column<int>(type: "int", nullable: true),
                    StrengthMinimum = table.Column<int>(type: "int", nullable: true),
                    StealthDisadvantage = table.Column<bool>(type: "bit", nullable: true),
                    GearCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToolCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVariant = table.Column<bool>(type: "bit", nullable: false),
                    HasVariant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentVariants",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    VariantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentVariants", x => new { x.EquipmentId, x.VariantId });
                    table.ForeignKey(
                        name: "FK_EquipmentVariants_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentVariants_Equipment_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PackContents",
                columns: table => new
                {
                    PackId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackContents", x => new { x.PackId, x.ContentId });
                    table.ForeignKey(
                        name: "FK_PackContents_Equipment_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PackContents_Equipment_PackId",
                        column: x => x.PackId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentWeaponProperties",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    WeaponPropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentWeaponProperties", x => new { x.EquipmentId, x.WeaponPropertyId });
                    table.ForeignKey(
                        name: "FK_EquipmentWeaponProperties_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentWeaponProperties_WeaponProperties_WeaponPropertyId",
                        column: x => x.WeaponPropertyId,
                        principalTable: "WeaponProperties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentVariants_VariantId",
                table: "EquipmentVariants",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentWeaponProperties_WeaponPropertyId",
                table: "EquipmentWeaponProperties",
                column: "WeaponPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PackContents_ContentId",
                table: "PackContents",
                column: "ContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "EquipmentVariants");

            migrationBuilder.DropTable(
                name: "EquipmentWeaponProperties");

            migrationBuilder.DropTable(
                name: "PackContents");

            migrationBuilder.DropTable(
                name: "WeaponProperties");

            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
