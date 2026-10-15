namespace Beer_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartAndCartItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        Cart_CartId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Carts", t => t.Cart_CartId)
                .Index(t => t.Cart_CartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "Cart_CartId", "dbo.Carts");
            DropIndex("dbo.CartItems", new[] { "Cart_CartId" });
            DropTable("dbo.CartItems");
            DropTable("dbo.Carts");
        }
    }
}
