namespace Beer_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderForMoMoPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentStatus", c => c.String());
            AddColumn("dbo.Orders", "TransactionId", c => c.String());
            AddColumn("dbo.Orders", "PaymentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentDate");
            DropColumn("dbo.Orders", "TransactionId");
            DropColumn("dbo.Orders", "PaymentStatus");
        }
    }
}
