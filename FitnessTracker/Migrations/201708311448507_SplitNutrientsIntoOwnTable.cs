namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitNutrientsIntoOwnTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.FoodItems", new[] {"ID"});
            DropColumn("dbo.FoodItems", "ID");

            AddColumn("dbo.FoodItems", "FoodId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.FoodItems", "FoodId");
            
            DropColumn("dbo.FoodItems", "BaseWater");
            DropColumn("dbo.FoodItems", "BaseEnergy");
            DropColumn("dbo.FoodItems", "BaseProtein");
            DropColumn("dbo.FoodItems", "BaseLipid");
            DropColumn("dbo.FoodItems", "BaseCarb");
            DropColumn("dbo.FoodItems", "BaseSugar");
            DropColumn("dbo.FoodItems", "BaseCalcium");
            DropColumn("dbo.FoodItems", "BaseSodium");
            CreateTable(
    "dbo.Nutrients",
    c => new
    {
        NutrientId = c.Int(nullable: false, identity: true),
        Name = c.String(),
        Unit = c.String(),
        Value = c.Double(nullable: false),
        FoodId = c.Int(nullable: false),
    })
    .PrimaryKey(t => t.NutrientId)
    .ForeignKey("dbo.FoodItems", t => t.FoodId, cascadeDelete: true)
    .Index(t => t.FoodId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nutrients", "FoodId", "dbo.FoodItems");
            DropIndex("dbo.Nutrients", new[] { "FoodId" });
            DropPrimaryKey("dbo.FoodItems");
            DropColumn("dbo.FoodItems", "FoodId");
            DropTable("dbo.Nutrients");
            AddColumn("dbo.FoodItems", "BaseSodium", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseCalcium", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseSugar", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseCarb", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseLipid", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseProtein", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseEnergy", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "BaseWater", c => c.Double(nullable: false));
            AddColumn("dbo.FoodItems", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.FoodItems", "ID");
        }
    }
}
