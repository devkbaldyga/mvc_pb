namespace WebApplication44.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferId = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        LocationID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        Title = c.String(),
                        Descritpion = c.String(),
                        Price = c.Double(nullable: false),
                        Address = c.String(),
                        Phone = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OfferId)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offers", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Offers", new[] { "CategoryID" });
            DropIndex("dbo.Offers", new[] { "UserID" });
            DropTable("dbo.Offers");
            DropTable("dbo.Categories");
        }
    }
}
