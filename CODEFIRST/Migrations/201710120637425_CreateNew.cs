namespace CODEFIRST.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        Heading = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CarViewModels");
        }
    }
}
