namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixWednesdaySpelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routines", "Wednesday", c => c.Boolean(nullable: false));
            DropColumn("dbo.Routines", "Wednesay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routines", "Wednesay", c => c.Boolean(nullable: false));
            DropColumn("dbo.Routines", "Wednesday");
        }
    }
}
