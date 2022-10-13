namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OCForVoucher : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vouchers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Vouchers", new[] { "UserId" });
            AlterColumn("dbo.Vouchers", "VoucherNo", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Vouchers", "CreateDate", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Vouchers", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Vouchers", "UserId");
            AddForeignKey("dbo.Vouchers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vouchers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Vouchers", new[] { "UserId" });
            AlterColumn("dbo.Vouchers", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Vouchers", "CreateDate", c => c.String());
            AlterColumn("dbo.Vouchers", "VoucherNo", c => c.String());
            CreateIndex("dbo.Vouchers", "UserId");
            AddForeignKey("dbo.Vouchers", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
