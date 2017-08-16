namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExerciseAddSets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Sets", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "Sets");
        }
    }
}
