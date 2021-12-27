namespace eInvoicing.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1st_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DocumentType = c.String(),
                        DocumentTypeVersion = c.String(),
                        ProformaInvoiceNumber = c.String(),
                        TotalDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        TotalSalesAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        ExtraDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        TotalItemsDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
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
                        IssuerGovernorate = c.String(),
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
                        ReceiverGovernorate = c.String(),
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
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 5),
                        InternalCode = c.String(),
                        SalesTotal = c.Decimal(nullable: false, precision: 18, scale: 5),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 5),
                        ValueDifference = c.Decimal(nullable: false, precision: 18, scale: 5),
                        TotalTaxableFees = c.Decimal(nullable: false, precision: 18, scale: 5),
                        NetTotal = c.Decimal(nullable: false, precision: 18, scale: 5),
                        ItemsDiscount = c.Decimal(nullable: false, precision: 18, scale: 5),
                        Description = c.String(),
                        CurrencySold = c.String(),
                        AmountEGP = c.Decimal(nullable: false, precision: 18, scale: 5),
                        AmountSold = c.Decimal(nullable: false, precision: 18, scale: 5),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 5),
                        DiscountRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 5),
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
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 5),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaxableItems", "InvoiceLineId", "dbo.InvoiceLines");
            DropForeignKey("dbo.InvoiceLines", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Errors", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "ParentId", "dbo.Documents");
            DropIndex("dbo.TaxableItems", new[] { "InvoiceLineId" });
            DropIndex("dbo.InvoiceLines", new[] { "DocumentId" });
            DropIndex("dbo.Errors", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "ParentId" });
            DropTable("dbo.TaxableItems");
            DropTable("dbo.InvoiceLines");
            DropTable("dbo.Errors");
            DropTable("dbo.Documents");
        }
    }
}
