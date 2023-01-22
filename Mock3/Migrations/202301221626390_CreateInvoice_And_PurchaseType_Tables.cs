namespace Mock3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInvoice_And_PurchaseType_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.String(nullable: false, maxLength: 10),
                        Description = c.String(),
                        PurchaseTypeId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseTypes", t => t.PurchaseTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PurchaseTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PurchaseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invoices", "PurchaseTypeId", "dbo.PurchaseTypes");
            DropIndex("dbo.Invoices", new[] { "UserId" });
            DropIndex("dbo.Invoices", new[] { "PurchaseTypeId" });
            DropTable("dbo.PurchaseTypes");
            DropTable("dbo.Invoices");
        }
    }
}
