using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class CreateUrgentScoresTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UrgentScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        SubmitDate = c.String(nullable: false, maxLength: 10),
                        InvoiceId = c.Int(nullable: false),
                        UserExamId = c.Int(nullable: false),
                        VoucherId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UrgentScores");
        }
    }
}
