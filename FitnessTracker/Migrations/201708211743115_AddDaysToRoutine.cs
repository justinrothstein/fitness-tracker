namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDaysToRoutine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routines", "Sunday", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Monday", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Tuesday", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Wednesay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Thursday", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Friday", c => c.Boolean(nullable: false));
            AddColumn("dbo.Routines", "Saturday", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routines", "Saturday");
            DropColumn("dbo.Routines", "Friday");
            DropColumn("dbo.Routines", "Thursday");
            DropColumn("dbo.Routines", "Wednesay");
            DropColumn("dbo.Routines", "Tuesday");
            DropColumn("dbo.Routines", "Monday");
            DropColumn("dbo.Routines", "Sunday");
        }
    }
}
