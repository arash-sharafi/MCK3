namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
