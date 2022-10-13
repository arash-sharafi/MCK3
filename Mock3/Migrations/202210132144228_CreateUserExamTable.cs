namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserExamTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserExams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChairNo = c.Byte(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ExamId = c.Int(nullable: false),
                        VoucherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Vouchers", t => t.VoucherId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ExamId)
                .Index(t => t.VoucherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserExams", "VoucherId", "dbo.Vouchers");
            DropForeignKey("dbo.UserExams", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserExams", "ExamId", "dbo.Exams");
            DropIndex("dbo.UserExams", new[] { "VoucherId" });
            DropIndex("dbo.UserExams", new[] { "ExamId" });
            DropIndex("dbo.UserExams", new[] { "UserId" });
            DropTable("dbo.UserExams");
        }
    }
}
