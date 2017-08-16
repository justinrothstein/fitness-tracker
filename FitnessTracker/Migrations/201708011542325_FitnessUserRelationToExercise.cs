namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FitnessUserRelationToExercise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Username", c => c.String(maxLength: 128));
            CreateIndex("dbo.Exercises", "Username");
            AddForeignKey("dbo.Exercises", "Username", "dbo.FitnessUsers", "Username");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exercises", "Username", "dbo.FitnessUsers");
            DropIndex("dbo.Exercises", new[] { "Username" });
            DropColumn("dbo.Exercises", "Username");
        }
    }
}
