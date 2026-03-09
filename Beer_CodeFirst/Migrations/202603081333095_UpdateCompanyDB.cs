namespace Beer_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCompanyDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "OriginalPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "AlcoholContent", c => c.Double());
            AlterColumn("dbo.Products", "Volume", c => c.Int());
            AlterColumn("dbo.Products", "StockQuantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "StockQuantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Volume", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "AlcoholContent", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "OriginalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
