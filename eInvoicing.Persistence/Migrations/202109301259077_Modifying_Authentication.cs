namespace eInvoicing.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifying_Authentication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RolePrivilegePermissions", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.RolePrivilegePermissions", "RolePrivilegeId", "dbo.RolePrivileges");
            DropForeignKey("dbo.RolePrivilegePermissions", "Privilege_Id", "dbo.Privileges");
            DropForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges");
            DropForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles");
            DropIndex("dbo.RolePrivileges", new[] { "RoleId" });
            DropIndex("dbo.RolePrivileges", new[] { "PrivilegeId" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "RolePrivilegeId" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "PermissionId" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "Privilege_Id" });
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            DropTable("dbo.RolePrivileges");
            DropTable("dbo.Privileges");
            DropTable("dbo.RolePrivilegePermissions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RolePrivilegePermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RolePrivilegeId = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                        Privilege_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Controller = c.String(),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePrivileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PrivilegeId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.RolePermissions", new[] { "PermissionId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropTable("dbo.RolePermissions");
            CreateIndex("dbo.RolePrivilegePermissions", "Privilege_Id");
            CreateIndex("dbo.RolePrivilegePermissions", "PermissionId");
            CreateIndex("dbo.RolePrivilegePermissions", "RolePrivilegeId");
            CreateIndex("dbo.RolePrivileges", "PrivilegeId");
            CreateIndex("dbo.RolePrivileges", "RoleId");
            AddForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePrivilegePermissions", "Privilege_Id", "dbo.Privileges", "Id");
            AddForeignKey("dbo.RolePrivilegePermissions", "RolePrivilegeId", "dbo.RolePrivileges", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePrivilegePermissions", "PermissionId", "dbo.Permissions", "Id", cascadeDelete: true);
        }
    }
}
