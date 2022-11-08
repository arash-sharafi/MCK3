namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateExamTitlesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExamTitles");
        }
    }
}
