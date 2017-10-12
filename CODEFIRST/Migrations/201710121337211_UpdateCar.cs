namespace CODEFIRST.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarViewModels", "Employee", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarViewModels", "Employee");
        }
    }
}
