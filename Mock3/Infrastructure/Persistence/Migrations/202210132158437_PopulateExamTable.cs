using System.Data.Entity.Migrations;

namespace Mock3.Infrastructure.Persistence.Migrations
{
    public partial class PopulateExamTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO EXAMS (Name, StartDate, Capacity, RemainingCapacity, IsOpen)" +
                " VALUES ('آزمون شماره 1','1396/01/01',50,50,1)");
            Sql("INSERT INTO EXAMS (Name, StartDate, Capacity, RemainingCapacity, IsOpen)" +
                 " VALUES ('آزمون شماره 2','1396/01/08',50,50,1)");
            Sql("INSERT INTO EXAMS (Name, StartDate, Capacity, RemainingCapacity, IsOpen)" +
                  " VALUES ('آزمون شماره 3','1396/01/15',50,50,1)");
            Sql("INSERT INTO EXAMS (Name, StartDate, Capacity, RemainingCapacity, IsOpen)" +
                   " VALUES ('آزمون شماره 4','1396/01/23',50,50,1)");
        }

        public override void Down()
        {
            Sql("DELETE FROM EXAMS WHERE Id IN (1,2,3,4)");
        }
    }
}
