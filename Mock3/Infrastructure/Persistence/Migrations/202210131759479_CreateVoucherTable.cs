using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class CreateVoucherTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VoucherNo = c.String(),
                        CreateDate = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vouchers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Vouchers", new[] { "UserId" });
            DropTable("dbo.Vouchers");
        }
    }
}
