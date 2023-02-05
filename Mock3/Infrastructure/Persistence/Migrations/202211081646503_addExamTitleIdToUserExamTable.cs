using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class addExamTitleIdToUserExamTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserExams", "ExamTitleId", c => c.Int());
            CreateIndex("dbo.UserExams", "ExamTitleId");
            AddForeignKey("dbo.UserExams", "ExamTitleId", "dbo.ExamTitles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserExams", "ExamTitleId", "dbo.ExamTitles");
            DropIndex("dbo.UserExams", new[] { "ExamTitleId" });
            DropColumn("dbo.UserExams", "ExamTitleId");
        }
    }
}
