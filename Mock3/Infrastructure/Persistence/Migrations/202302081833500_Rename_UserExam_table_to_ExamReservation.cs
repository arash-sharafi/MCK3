namespace Mock3.Infrastructure.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_UserExam_table_to_ExamReservation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserExams", newName: "ExamReservations");
            AddColumn("dbo.UrgentScores", "ExamReservationId", c => c.Int(nullable: false));
            DropColumn("dbo.UrgentScores", "UserExamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UrgentScores", "UserExamId", c => c.Int(nullable: false));
            DropColumn("dbo.UrgentScores", "ExamReservationId");
            RenameTable(name: "dbo.ExamReservations", newName: "UserExams");
        }
    }
}
