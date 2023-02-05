using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class CreateExamScoreTable : DbMigration
    {
        public override void Up()
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
                        SubmitDate = c.String(),
                        UserExamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserExams", t => t.UserExamId, cascadeDelete: true)
                .Index(t => t.UserExamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamScores", "UserExamId", "dbo.UserExams");
            DropIndex("dbo.ExamScores", new[] { "UserExamId" });
            DropTable("dbo.ExamScores");
        }
    }
}
