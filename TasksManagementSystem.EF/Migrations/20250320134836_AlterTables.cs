using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksManagementSystem.EF.Migrations
{
    /// <inheritdoc />
    public partial class AlterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_AspNetUsers_CreatedById",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_TaskEntity_TaskId",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntity_AspNetUsers_CreatedById",
                table: "ProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskEntity_AspNetUsers_AssignedToId",
                table: "TaskEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskEntity_ProjectEntity_ProjectId",
                table: "TaskEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskEntity",
                table: "TaskEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity");

            migrationBuilder.RenameTable(
                name: "TaskEntity",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "ProjectEntity",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "CommentEntity",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_TaskEntity_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskEntity_AssignedToId",
                table: "Tasks",
                newName: "IX_Tasks_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEntity_CreatedById",
                table: "Projects",
                newName: "IX_Projects_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_TaskId",
                table: "Comments",
                newName: "IX_Comments_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_CreatedById",
                table: "Comments",
                newName: "IX_Comments_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToId",
                table: "Tasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskEntity");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "ProjectEntity");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "TaskEntity",
                newName: "IX_TaskEntity_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignedToId",
                table: "TaskEntity",
                newName: "IX_TaskEntity_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CreatedById",
                table: "ProjectEntity",
                newName: "IX_ProjectEntity_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_TaskId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatedById",
                table: "CommentEntity",
                newName: "IX_CommentEntity_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskEntity",
                table: "TaskEntity",
                column: "TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity",
                column: "ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_AspNetUsers_CreatedById",
                table: "CommentEntity",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_TaskEntity_TaskId",
                table: "CommentEntity",
                column: "TaskId",
                principalTable: "TaskEntity",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntity_AspNetUsers_CreatedById",
                table: "ProjectEntity",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEntity_AspNetUsers_AssignedToId",
                table: "TaskEntity",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEntity_ProjectEntity_ProjectId",
                table: "TaskEntity",
                column: "ProjectId",
                principalTable: "ProjectEntity",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
