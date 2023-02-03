namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OCForExamScore : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExamScores", "SubmitDate", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExamScores", "SubmitDate", c => c.String());
        }
    }
}
