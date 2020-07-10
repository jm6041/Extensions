using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore3App.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(fixedLength: true, maxLength: 32, nullable: false, comment: "Id"),
                    Name = table.Column<string>(maxLength: 100, nullable: false, comment: "名字")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, comment: "Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false, comment: "名字"),
                    Created = table.Column<DateTimeOffset>(nullable: false, comment: "创建时间"),
                    Sex = table.Column<int>(nullable: false, comment: "性别"),
                    IntV = table.Column<int>(nullable: false, comment: "整形"),
                    DouV = table.Column<double>(nullable: false, comment: "浮点数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Created", "DouV", "IntV", "Name", "Sex" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 0.10000000000000001, 0, "Test0", 0 },
                    { 73, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 72.099999999999994, 72, "Test72", 0 },
                    { 72, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 71.099999999999994, 71, "Test71", 0 },
                    { 71, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 70.099999999999994, 70, "Test70", 0 },
                    { 70, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 69.099999999999994, 69, "Test69", 0 },
                    { 69, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 68.099999999999994, 68, "Test68", 0 },
                    { 68, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 67.099999999999994, 67, "Test67", 0 },
                    { 67, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 66.099999999999994, 66, "Test66", 0 },
                    { 66, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 65.099999999999994, 65, "Test65", 0 },
                    { 65, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 64.099999999999994, 64, "Test64", 0 },
                    { 64, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 63.100000000000001, 63, "Test63", 0 },
                    { 63, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 62.100000000000001, 62, "Test62", 0 },
                    { 62, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 61.100000000000001, 61, "Test61", 0 },
                    { 61, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 60.100000000000001, 60, "Test60", 0 },
                    { 60, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 59.100000000000001, 59, "Test59", 0 },
                    { 59, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 58.100000000000001, 58, "Test58", 0 },
                    { 58, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 57.100000000000001, 57, "Test57", 0 },
                    { 57, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 56.100000000000001, 56, "Test56", 0 },
                    { 56, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 55.100000000000001, 55, "Test55", 0 },
                    { 55, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 54.100000000000001, 54, "Test54", 0 },
                    { 54, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 53.100000000000001, 53, "Test53", 0 },
                    { 53, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 52.100000000000001, 52, "Test52", 0 },
                    { 74, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 73.099999999999994, 73, "Test73", 0 },
                    { 52, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 51.100000000000001, 51, "Test51", 0 },
                    { 75, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 74.099999999999994, 74, "Test74", 0 },
                    { 77, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 76.099999999999994, 76, "Test76", 0 },
                    { 98, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 97.099999999999994, 97, "Test97", 0 },
                    { 97, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 96.099999999999994, 96, "Test96", 0 },
                    { 96, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 95.099999999999994, 95, "Test95", 0 },
                    { 95, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 94.099999999999994, 94, "Test94", 0 },
                    { 94, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 93.099999999999994, 93, "Test93", 0 },
                    { 93, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 92.099999999999994, 92, "Test92", 0 },
                    { 92, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 91.099999999999994, 91, "Test91", 0 },
                    { 91, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 90.099999999999994, 90, "Test90", 0 },
                    { 90, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 89.099999999999994, 89, "Test89", 0 },
                    { 89, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 88.099999999999994, 88, "Test88", 0 },
                    { 88, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 87.099999999999994, 87, "Test87", 0 },
                    { 87, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 86.099999999999994, 86, "Test86", 0 },
                    { 86, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 85.099999999999994, 85, "Test85", 0 },
                    { 85, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 84.099999999999994, 84, "Test84", 0 },
                    { 84, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 83.099999999999994, 83, "Test83", 0 },
                    { 83, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 82.099999999999994, 82, "Test82", 0 },
                    { 82, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 81.099999999999994, 81, "Test81", 0 },
                    { 81, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 80.099999999999994, 80, "Test80", 0 },
                    { 80, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 79.099999999999994, 79, "Test79", 0 },
                    { 79, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 78.099999999999994, 78, "Test78", 0 },
                    { 78, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 77.099999999999994, 77, "Test77", 0 },
                    { 76, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 75.099999999999994, 75, "Test75", 0 },
                    { 51, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 50.100000000000001, 50, "Test50", 0 },
                    { 50, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 49.100000000000001, 49, "Test49", 0 },
                    { 49, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 48.100000000000001, 48, "Test48", 0 },
                    { 22, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 21.100000000000001, 21, "Test21", 0 },
                    { 21, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 20.100000000000001, 20, "Test20", 0 },
                    { 20, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 19.100000000000001, 19, "Test19", 0 },
                    { 19, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 18.100000000000001, 18, "Test18", 0 },
                    { 18, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 17.100000000000001, 17, "Test17", 0 },
                    { 17, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 16.100000000000001, 16, "Test16", 0 },
                    { 16, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 15.1, 15, "Test15", 0 },
                    { 15, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 14.1, 14, "Test14", 0 },
                    { 14, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 13.1, 13, "Test13", 0 },
                    { 13, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 12.1, 12, "Test12", 0 },
                    { 12, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 11.1, 11, "Test11", 0 },
                    { 11, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 10.1, 10, "Test10", 0 },
                    { 10, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 9.0999999999999996, 9, "Test9", 0 },
                    { 9, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 8.0999999999999996, 8, "Test8", 0 },
                    { 8, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 7.0999999999999996, 7, "Test7", 0 },
                    { 7, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 6.0999999999999996, 6, "Test6", 0 },
                    { 6, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 5.0999999999999996, 5, "Test5", 0 },
                    { 5, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 4.0999999999999996, 4, "Test4", 0 },
                    { 4, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 3.1000000000000001, 3, "Test3", 0 },
                    { 3, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 2.1000000000000001, 2, "Test2", 0 },
                    { 2, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 1.1000000000000001, 1, "Test1", 0 },
                    { 23, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 22.100000000000001, 22, "Test22", 0 },
                    { 24, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 23.100000000000001, 23, "Test23", 0 },
                    { 25, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 24.100000000000001, 24, "Test24", 0 },
                    { 26, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 25.100000000000001, 25, "Test25", 0 },
                    { 48, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 47.100000000000001, 47, "Test47", 0 },
                    { 47, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 46.100000000000001, 46, "Test46", 0 },
                    { 46, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 45.100000000000001, 45, "Test45", 0 },
                    { 45, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 44.100000000000001, 44, "Test44", 0 },
                    { 44, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 43.100000000000001, 43, "Test43", 0 },
                    { 43, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 42.100000000000001, 42, "Test42", 0 },
                    { 42, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 41.100000000000001, 41, "Test41", 0 },
                    { 41, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 40.100000000000001, 40, "Test40", 0 },
                    { 40, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 39.100000000000001, 39, "Test39", 0 },
                    { 39, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 38.100000000000001, 38, "Test38", 0 },
                    { 99, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 98.099999999999994, 98, "Test98", 0 },
                    { 38, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 37.100000000000001, 37, "Test37", 0 },
                    { 36, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 35.100000000000001, 35, "Test35", 0 },
                    { 35, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 34.100000000000001, 34, "Test34", 0 },
                    { 34, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 33.100000000000001, 33, "Test33", 0 },
                    { 33, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 32.100000000000001, 32, "Test32", 0 },
                    { 32, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 31.100000000000001, 31, "Test31", 0 },
                    { 31, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 30.100000000000001, 30, "Test30", 0 },
                    { 30, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 29.100000000000001, 29, "Test29", 0 },
                    { 29, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 28.100000000000001, 28, "Test28", 0 },
                    { 28, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 27.100000000000001, 27, "Test27", 0 },
                    { 27, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 26.100000000000001, 26, "Test26", 0 },
                    { 37, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 36.100000000000001, 36, "Test36", 0 },
                    { 100, new DateTimeOffset(new DateTime(2020, 3, 12, 12, 33, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), 99.099999999999994, 99, "Test99", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "OrderNameIndex",
                table: "Order",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
