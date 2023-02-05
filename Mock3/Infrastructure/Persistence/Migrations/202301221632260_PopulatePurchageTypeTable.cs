using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class PopulatePurchageTypeTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO PURCHASETYPES (Id, Name)" +
                " VALUES (1,'Buy a Voucher')");
            Sql("INSERT INTO PURCHASETYPES (Id, Name)" +
                " VALUES (2,'Release a Voucher')");
            Sql("INSERT INTO PURCHASETYPES (Id, Name)" +
                " VALUES (3,'Buy an Urgent Score service')");
        }
        
        public override void Down()
        {
        }
    }
}
