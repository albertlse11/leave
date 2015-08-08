namespace LeaveApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 200),
                        Deleted = c.Boolean(nullable: false),
                        UserCreated = c.String(),
                        DateCreated = c.DateTime(),
                        UserModified = c.String(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Leaves",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LeaveDate = c.DateTime(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        LTypeID = c.Int(nullable: false),
                        ReasonID = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        UserCreated = c.String(),
                        DateCreated = c.DateTime(),
                        UserModified = c.String(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                        UserCreated = c.String(),
                        DateCreated = c.DateTime(),
                        UserModified = c.String(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reasons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Desc = c.String(nullable: false, maxLength: 200),
                        Deleted = c.Boolean(nullable: false),
                        UserCreated = c.String(),
                        DateCreated = c.DateTime(),
                        UserModified = c.String(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reasons");
            DropTable("dbo.LTypes");
            DropTable("dbo.Leaves");
            DropTable("dbo.Employees");
        }
    }
}
