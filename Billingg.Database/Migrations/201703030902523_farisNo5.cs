namespace Billingg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class farisNo5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Suppliers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Suppliers", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Deleted");
            DropColumn("dbo.Suppliers", "CreatedOn");
            DropColumn("dbo.Suppliers", "CreatedBy");
        }
    }
}
