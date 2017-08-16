namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWeightToExercise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Weight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "Weight");
        }
    }
}
