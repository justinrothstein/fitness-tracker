namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addactivevalueforroutine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routines", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routines", "Active");
        }
    }
}
