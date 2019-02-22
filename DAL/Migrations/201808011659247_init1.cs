namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Advertisings");
            AlterColumn("dbo.Advertisings", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Advertisings", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Advertisings");
            AlterColumn("dbo.Advertisings", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Advertisings", "Id");
        }
    }
}
