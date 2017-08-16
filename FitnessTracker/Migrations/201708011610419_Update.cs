namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExerciseRoutines", newName: "RoutineExercises");
            DropPrimaryKey("dbo.RoutineExercises");
            AddPrimaryKey("dbo.RoutineExercises", new[] { "Routine_RoutineID", "Exercise_ExerciseID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RoutineExercises");
            AddPrimaryKey("dbo.RoutineExercises", new[] { "Exercise_ExerciseID", "Routine_RoutineID" });
            RenameTable(name: "dbo.RoutineExercises", newName: "ExerciseRoutines");
        }
    }
}
