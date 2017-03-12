namespace RubyTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additional_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "LastModifiedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String());
            DropColumn("dbo.Projects", "LastModifiedAt");
        }
    }
}
