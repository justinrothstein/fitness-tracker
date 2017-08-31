namespace FitnessTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcutallyAddFoodTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UsdaDbNum = c.String(),
                        DateAdded = c.String(),
                        ServingAmount = c.Double(nullable: false),
                        ServingLabel = c.String(),
                        ServingsConsumed = c.Double(nullable: false),
                        BaseWater = c.Double(nullable: false),
                        BaseEnergy = c.Double(nullable: false),
                        BaseProtein = c.Double(nullable: false),
                        BaseLipid = c.Double(nullable: false),
                        BaseCarb = c.Double(nullable: false),
                        BaseSugar = c.Double(nullable: false),
                        BaseCalcium = c.Double(nullable: false),
                        BaseSodium = c.Double(nullable: false),
                        Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FitnessUsers", t => t.Username)
                .Index(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodItems", "Username", "dbo.FitnessUsers");
            DropIndex("dbo.FoodItems", new[] { "Username" });
            DropTable("dbo.FoodItems");
        }
    }
}
