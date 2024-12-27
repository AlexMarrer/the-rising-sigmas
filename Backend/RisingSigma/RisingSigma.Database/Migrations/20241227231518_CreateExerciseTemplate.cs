using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RisingSigma.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateExerciseTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExerciseTemplateId",
                table: "Exercise",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseTemplate_MuscleGroup_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ExerciseTemplateId",
                table: "Exercise",
                column: "ExerciseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTemplate_MuscleGroupId",
                table: "ExerciseTemplate",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_ExerciseTemplate_ExerciseTemplateId",
                table: "Exercise",
                column: "ExerciseTemplateId",
                principalTable: "ExerciseTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_ExerciseTemplate_ExerciseTemplateId",
                table: "Exercise");

            migrationBuilder.DropTable(
                name: "ExerciseTemplate");

            migrationBuilder.DropTable(
                name: "MuscleGroup");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_ExerciseTemplateId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "ExerciseTemplateId",
                table: "Exercise");
        }
    }
}
