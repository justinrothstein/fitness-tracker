namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExerciseRoutineRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        ExerciseID = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(nullable: false),
                        Description = c.String(),
                        Reps = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExerciseID);
            
            CreateTable(
                "dbo.ExerciseRoutines",
                c => new
                    {
                        Exercise_ExerciseID = c.Int(nullable: false),
                        Routine_RoutineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exercise_ExerciseID, t.Routine_RoutineID })
                .ForeignKey("dbo.Exercises", t => t.Exercise_ExerciseID, cascadeDelete: true)
                .ForeignKey("dbo.Routines", t => t.Routine_RoutineID, cascadeDelete: true)
                .Index(t => t.Exercise_ExerciseID)
                .Index(t => t.Routine_RoutineID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExerciseRoutines", "Routine_RoutineID", "dbo.Routines");
            DropForeignKey("dbo.ExerciseRoutines", "Exercise_ExerciseID", "dbo.Exercises");
            DropIndex("dbo.ExerciseRoutines", new[] { "Routine_RoutineID" });
            DropIndex("dbo.ExerciseRoutines", new[] { "Exercise_ExerciseID" });
            DropTable("dbo.ExerciseRoutines");
            DropTable("dbo.Exercises");
        }
    }
}
