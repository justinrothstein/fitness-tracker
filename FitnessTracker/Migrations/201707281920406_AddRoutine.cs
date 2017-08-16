namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoutine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routines",
                c => new
                    {
                        RoutineID = c.Int(nullable: false, identity: true),
                        RoutineName = c.String(),
                        RoutineGoal = c.String(),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoutineID)
                .ForeignKey("dbo.FitnessUsers", t => t.User_Username)
                .Index(t => t.User_Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routines", "User_Username", "dbo.FitnessUsers");
            DropIndex("dbo.Routines", new[] { "User_Username" });
            DropTable("dbo.Routines");
        }
    }
}
