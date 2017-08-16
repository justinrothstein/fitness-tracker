namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRoutine : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Routines", "User_Username", "dbo.FitnessUsers");
            DropIndex("dbo.Routines", new[] { "User_Username" });
            RenameColumn(table: "dbo.Routines", name: "User_Username", newName: "Username");
            AlterColumn("dbo.Routines", "RoutineName", c => c.String(nullable: false));
            AlterColumn("dbo.Routines", "Username", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Routines", "Username");
            AddForeignKey("dbo.Routines", "Username", "dbo.FitnessUsers", "Username", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routines", "Username", "dbo.FitnessUsers");
            DropIndex("dbo.Routines", new[] { "Username" });
            AlterColumn("dbo.Routines", "Username", c => c.String(maxLength: 128));
            AlterColumn("dbo.Routines", "RoutineName", c => c.String());
            RenameColumn(table: "dbo.Routines", name: "Username", newName: "User_Username");
            CreateIndex("dbo.Routines", "User_Username");
            AddForeignKey("dbo.Routines", "User_Username", "dbo.FitnessUsers", "Username");
        }
    }
}
