namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserExamTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExamScores", "UserExamId", "dbo.UserExams");
            DropIndex("dbo.ExamScores", new[] { "UserExamId" });
            AddColumn("dbo.UserExams", "ReadingScore", c => c.Double(nullable: false));
            AddColumn("dbo.UserExams", "ListeningScore", c => c.Double(nullable: false));
            AddColumn("dbo.UserExams", "SpeakingScore", c => c.Double(nullable: false));
            AddColumn("dbo.UserExams", "WritingScore", c => c.Double(nullable: false));
            AddColumn("dbo.UserExams", "ScoreSubmitDate", c => c.String(maxLength: 10));
            DropTable("dbo.ExamScores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExamScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reading = c.Double(nullable: false),
                        Listening = c.Double(nullable: false),
                        Speaking = c.Double(nullable: false),
                        Writing = c.Double(nullable: false),
                        SubmitDate = c.String(maxLength: 10),
                        UserExamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.UserExams", "ScoreSubmitDate");
            DropColumn("dbo.UserExams", "WritingScore");
            DropColumn("dbo.UserExams", "SpeakingScore");
            DropColumn("dbo.UserExams", "ListeningScore");
            DropColumn("dbo.UserExams", "ReadingScore");
            CreateIndex("dbo.ExamScores", "UserExamId");
            AddForeignKey("dbo.ExamScores", "UserExamId", "dbo.UserExams", "Id", cascadeDelete: true);
        }
    }
}
