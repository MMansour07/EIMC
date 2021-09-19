namespace eInvoicing.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.Binary(),
                        PasswordSalt = c.Binary(),
                        PhoneNumber = c.String(),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Privileges", t => t.PrivilegeId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PrivilegeId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.RolePrivileges", t => t.RolePrivilegeId, cascadeDelete: true)
                .ForeignKey("dbo.Privileges", t => t.Privilege_Id)
                .Index(t => t.RolePrivilegeId)
                .Index(t => t.PermissionId)
                .Index(t => t.Privilege_Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Action = c.String(),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DocumentType = c.String(),
                        DocumentTypeVersion = c.String(),
                        ProformaInvoiceNumber = c.String(),
                        TotalDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExtraDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalItemsDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxpayerActivityCode = c.String(),
                        PurchaseOrderReference = c.String(),
                        PurchaseOrderDescription = c.String(),
                        SalesOrderReference = c.String(),
                        SalesOrderDescription = c.String(),
                        IssuerId = c.String(),
                        IssuerName = c.String(),
                        IssuerType = c.String(),
                        IssuerBranchId = c.String(),
                        IssuerCountry = c.String(),
                        IssuerGovernate = c.String(),
                        IssuerRegionCity = c.String(),
                        IssuerStreet = c.String(),
                        IssuerBuildingNumber = c.String(),
                        IssuerPostalCode = c.String(),
                        IssuerFloor = c.String(),
                        IssuerRoom = c.String(),
                        IssuerLandmark = c.String(),
                        IssuerAdditionalInformation = c.String(),
                        ReceiverId = c.String(),
                        ReceiverName = c.String(),
                        ReceiverType = c.String(),
                        ReceiverCountry = c.String(),
                        ReceiverGovernate = c.String(),
                        ReceiverRegionCity = c.String(),
                        ReceiverStreet = c.String(),
                        ReceiverBuildingNumber = c.String(),
                        ReceiverPostalCode = c.String(),
                        ReceiverFloor = c.String(),
                        ReceiverRoom = c.String(),
                        ReceiverLandmark = c.String(),
                        ReceiverAdditionalInformation = c.String(),
                        BankName = c.String(),
                        BankAddress = c.String(),
                        BankAccountNo = c.String(),
                        BankAccountIBAN = c.String(),
                        SwiftCode = c.String(),
                        PaymentTerms = c.String(),
                        Approach = c.String(),
                        Packaging = c.String(),
                        DateValidity = c.DateTime(),
                        ExportPort = c.String(),
                        CountryOfOrigin = c.String(),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTerms = c.String(),
                        DateTimeIssued = c.DateTime(nullable: false),
                        DateTimeReceived = c.DateTime(nullable: false),
                        uuid = c.String(),
                        submissionId = c.String(),
                        longId = c.String(),
                        Status = c.String(),
                        SubmittedBy = c.String(),
                        InvalidReason = c.String(),
                        ParentId = c.String(maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        target = c.String(),
                        propertyPath = c.String(),
                        code = c.String(),
                        DocumentId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.InvoiceLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ItemType = c.String(),
                        ItemCode = c.String(),
                        UnitType = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InternalCode = c.String(),
                        SalesTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValueDifference = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalTaxableFees = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemsDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        CurrencySold = c.String(),
                        AmountEGP = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountSold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocumentId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.TaxableItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InternalId = c.String(),
                        TaxType = c.String(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubType = c.String(),
                        InvoiceLineId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoiceLines", t => t.InvoiceLineId, cascadeDelete: true)
                .Index(t => t.InvoiceLineId);
            
            CreateTable(
                "dbo.Lookups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        CreatedBy = c.String(),
                        LastModifiedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaxableItems", "InvoiceLineId", "dbo.InvoiceLines");
            DropForeignKey("dbo.InvoiceLines", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Errors", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "ParentId", "dbo.Documents");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges");
            DropForeignKey("dbo.RolePrivilegePermissions", "Privilege_Id", "dbo.Privileges");
            DropForeignKey("dbo.RolePrivilegePermissions", "RolePrivilegeId", "dbo.RolePrivileges");
            DropForeignKey("dbo.RolePrivilegePermissions", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.TaxableItems", new[] { "InvoiceLineId" });
            DropIndex("dbo.InvoiceLines", new[] { "DocumentId" });
            DropIndex("dbo.Errors", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "ParentId" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "Privilege_Id" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "PermissionId" });
            DropIndex("dbo.RolePrivilegePermissions", new[] { "RolePrivilegeId" });
            DropIndex("dbo.RolePrivileges", new[] { "PrivilegeId" });
            DropIndex("dbo.RolePrivileges", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropTable("dbo.Lookups");
            DropTable("dbo.TaxableItems");
            DropTable("dbo.InvoiceLines");
            DropTable("dbo.Errors");
            DropTable("dbo.Documents");
            DropTable("dbo.Permissions");
            DropTable("dbo.RolePrivilegePermissions");
            DropTable("dbo.Privileges");
            DropTable("dbo.RolePrivileges");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.AppUsers");
        }
    }
}
