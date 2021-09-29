using eInvoicing.DomainEntities.Base;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.DTO.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace eInvoicing.Service.Helper.Extension
{
    internal static class MappingExtention
    {

        internal static TEntity ToEntity<TEntity>(this IDTO dto) where TEntity : IEntity
        {
            return AutoMapperConfiguration.Mapper.Map<TEntity>(dto);
        }
        internal static IList<TEntity> ToEntityList<TEntity>(this IEnumerable<IDTO> dtos)
            where TEntity : IEntity 
        {
            return AutoMapperConfiguration.Mapper.Map<IList<TEntity>>(dtos);
        }
        internal static PagedList<TDto> ToPagedList<TDto>(this PagedList<IEntity> entity)
            where TDto : IDTO
        {
            return AutoMapperConfiguration.Mapper.Map<PagedList<TDto>>(entity);
        }
        internal static TDto ToDto<TDto>(this IEntity entity) where TDto : IDTO
        {
            return AutoMapperConfiguration.Mapper.Map<TDto>(entity);
        }
        internal static IList<TDto> ToDtoList<TDto>(this IEnumerable<IEntity> entity)
            where TDto : IDTO
        {
            return AutoMapperConfiguration.Mapper.Map<IList<TDto>>(entity);
        }

        internal static CustomErrorDTO ToErrorDto(this DocumentRejectedDTO obj)
        {
            if (obj.Error.details != null)
            {
                return new CustomErrorDTO()
                {
                    details = obj.Error.details.Select(i => new CustomErrorDTO() {DocumentId = obj.internalId, code = i.code, message = i.message, propertyPath = i.propertyPath, target = i.target, CreatedOn = DateTime.Now}).ToList()
                };
            }
            else
            {
                return new CustomErrorDTO()
                {
                    code = obj.Error.code,
                    DocumentId = obj.internalId,
                    message = obj.Error.message,
                    propertyPath = obj.Error.propertyPath,
                    target = obj.Error.target,
                    CreatedOn = DateTime.Now
                };
            }
            
        }
        internal static T As<T>(this object obj)
        {
            return AutoMapperConfiguration.Mapper.Map(obj, default(T));
        }
        internal static DocumentVM ToDocumentVM(this Document document)
        {
            return new DocumentVM()
            {
                dateTimeIssued = document.DateTimeIssued,
                documentType = document.DocumentType,
                documentTypeVersion = document.DocumentTypeVersion,
                dateTimeReceived = string.IsNullOrEmpty(document.DateTimeReceived.ToString()) ? DateTime.Now : DateTime.Parse(document.DateTimeReceived.ToString()),
                uuid = document.uuid,
                longId = document.longId,
                submissionUUID = document.submissionId,
                internalID = document.Id,
                recordID = document.Id,
                status = document.Status,
                taxAmount = (document.TotalSalesAmount - document.NetAmount).ToString("N2"),
                netAmount = document.NetAmount.ToString("N2"),
                totalAmount = document.TotalAmount.ToString("N2"),
                totalDiscountAmount = document.TotalDiscountAmount.ToString("N2"),
                totalItemsDiscountAmount = document.TotalItemsDiscountAmount.ToString("N2"),
                totalSalesAmount = document.TotalSalesAmount.ToString("N2"),
                extraDiscountAmount = document.ExtraDiscountAmount.ToString("N2"),
                proformaInvoiceNumber = document.ProformaInvoiceNumber,
                purchaseOrderDescription = document.PurchaseOrderDescription,
                purchaseOrderReference = document.PurchaseOrderReference,
                salesOrderDescription = document.SalesOrderDescription,
                salesOrderReference = document.SalesOrderReference,
                taxpayerActivityCode = document.TaxpayerActivityCode,
                IssuerFullAddress = document.IssuerBuildingNumber + " " + document.IssuerStreet + ", " + document.IssuerRegionCity + ", " + document.IssuerGovernate + ", " + document.IssuerCountry,
                ReceiverFullAddress = document.ReceiverBuildingNumber + " " + document.ReceiverStreet + ", " + document.ReceiverRegionCity + ", " + document.ReceiverGovernate + ", " + document.ReceiverCountry,
                delivery = new DELIVERIESDTO()
                {
                    approach = document.Approach,
                    countryOfOrigin = document.CountryOfOrigin,
                    dateValidity = document.DateValidity.ToString() ?? "",
                    exportPort = document.ExportPort,
                    grossWeight = Convert.ToDecimal(document.GrossWeight),
                    netWeight = Convert.ToDecimal(document.NetWeight),
                    packaging = document.Packaging,
                    terms = document.DeliveryTerms
                },
                issuer = new ISSUERSDTO()
                {
                    address = new ISSUERADDESSESDTO()
                    {
                        additionalInformation = document.IssuerAdditionalInformation,
                        branchID = document.IssuerBranchId,
                        buildingNumber = document.IssuerBuildingNumber,
                        country = document.IssuerCountry,
                        floor = document.IssuerFloor,
                        governate = document.IssuerGovernate,
                        landmark = document.IssuerLandmark,
                        postalCode = document.IssuerPostalCode,
                        regionCity = document.IssuerRegionCity,
                        room = document.IssuerRoom,
                        street = document.IssuerStreet
                    },
                    id = document.IssuerId,
                    name = document.IssuerName,
                    type = document.IssuerType
                },
                payment = new PAYMENTSDTO()
                {
                    bankAccountIBAN = document.BankAccountIBAN,
                    bankAccountNo = document.BankAccountNo,
                    bankAddress = document.BankAddress,
                    bankName = document.BankName,
                    swiftCode = document.SwiftCode,
                    terms = document.PaymentTerms
                },
                invoiceLines = document.InvoiceLines.Select(i => new INVOICELINESVM()
                {
                    description = i.Description,
                    internalCode = i.InternalCode,
                    itemCode = i.ItemCode,
                    itemsDiscount = i.ItemsDiscount.ToString("N2"),
                    itemType = i.ItemType,
                    netTotal = i.NetTotal.ToString("N2"),
                    quantity = i.Quantity.ToString("N2"),
                    salesTotal = i.SalesTotal.ToString("N2"),
                    total = i.Total.ToString("N2"),
                    totalTax = i.TaxableItems.Sum(x => x.Amount).ToString("N2"),
                    totalTaxableFees = i.TotalTaxableFees.ToString("N2"),
                    unitType = i.UnitType,
                    valueDifference = i.ValueDifference.ToString("N2"),
                    internalId = i.Id,
                    taxableItems = i.TaxableItems.Select(x => new TAXABLEITEMSVM()
                    {
                        amount = x.Amount.ToString("N2"),
                        rate = x.Rate.ToString("N2"),
                        subType = x.SubType,
                        taxType = x.TaxType
                    }).ToList(),
                    discount = new DISCOUNTSVM() { amount = i.DiscountAmount.ToString("N2"), rate = i.DiscountRate.ToString("N0") },
                    unitValue = new UNITVALUESVM() { amountEGP = i.AmountEGP.ToString("N2"), amountSold = i.AmountSold.ToString("N2"), currencyExchangeRate = i.CurrencyExchangeRate.ToString("N2"), currencySold = i.CurrencySold }
                }).ToList(),
                taxTotals = document.InvoiceLines.SelectMany(b => b.TaxableItems)?.Distinct().GroupBy(o => o.TaxType).Select(x => new TAXTOTALSDTO() { amount = x.Sum(y => y.Amount), taxType = x.Select(e => e.TaxType).First() }).ToList(),
                receiver = new RECEIVERSDTO()
                {
                    address = new RECEIVERADDRESSESDTO()
                    {
                        additionalInformation = document.ReceiverAdditionalInformation,
                        buildingNumber = document.ReceiverBuildingNumber,
                        country = document.ReceiverCountry,
                        floor = document.ReceiverFloor,
                        governate = document.ReceiverGovernate,
                        landmark = document.ReceiverLandmark,
                        postalCode = document.ReceiverPostalCode,
                        regionCity = document.ReceiverRegionCity,
                        room = document.ReceiverRoom,
                        street = document.ReceiverStreet
                    },
                    id =   document.ReceiverId,
                    name = document.ReceiverName,
                    type = document.ReceiverType
                },
                errors = document.Errors.Select(i => new CustomErrorDTO() { id = i.Id, code = i.code, message = i.message, propertyPath = i.propertyPath, DocumentId = i.DocumentId, target = i.target, CreatedOn = i.CreatedOn}).ToList(),
                parentId = document.ParentId
                // signatures = new List<SIGNATURESDTO>() { new SIGNATURESDTO() { signatureType = "I", value = "MIII0QYJKoZIhvcNAQcCoIIIwjCCCL4CAQMxDTALBglghkgBZQMEAgEwCwYJKoZIhvcNAQcFoIIGDzCCBgswggPzoAMCAQICEB7WHdVfBczn8ZiawvdzGP0wDQYJKoZIhvcNAQELBQAwRDELMAkGA1UEBhMCRUcxFDASBgNVBAoTC0VneXB0IFRydXN0MR8wHQYDVQQDExZFZ3lwdCBUcnVzdCBTZWFsaW5nIENBMB4XDTIwMDkyODAwMDAwMFoXDTIxMDkyODIzNTk1OVowggFYMRgwFgYDVQRhDA9WQVRFRy02NzQ4NTk1NDUxIjAgBgNVBAsMGVRBWCBJRCAtIDIyNTUwMDAwODExMDAwMTAxJTAjBgNVBAsMHE5hdGlvbmFsIElEIC0gMjcxMDExMjIxMDEzNzQxcTBvBgNVBAoMaNi02LHZg9mHINin2YTYtdmI2YHZiiDZhNmE2KrYrNin2LHZhyDZiNin2YTYqtmI2LHZitiv2KfYqiDYudio2K/Yp9mE2LnYstmK2LIg2KfYqNix2KfZh9mK2YUg2KfZhNi12YjZgdmKMXEwbwYDVQQDDGjYtNix2YPZhyDYp9mE2LXZiNmB2Yog2YTZhNiq2KzYp9ix2Ycg2YjYp9mE2KrZiNix2YrYr9in2Kog2LnYqNiv2KfZhNi52LLZitiyINin2KjYsdin2YfZitmFINin2YTYtdmI2YHZijELMAkGA1UEBhMCRUcwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCccO0oSnJjeL3Ebf8pLON\u002Br2dUrn3o9y8pdxOLEV\u002BLcmVBYlM2fY01jk6vU4BLmPFoYBclwD/smbtrXvXMQeeTH\u002B/2z8VZrDrsZwx3GpF5Auu0k/eruUrGN1W8LqSkMsCcIgseODTbjkn9tACdtFkYkrbnmqRuA9Cxc0kenscYTvtj4iUVjmJSnUK32c41kGQYmXyBCyfMKcxGFiF8\u002Bogg74CELrtVJfYA3toFGieRrD2JM\u002BziqbxfwjjtYayMHg\u002BPaOH06Qh/3JW/FyeQyRm3HYgxKxEGSMtPJAw/PsfqvsWOP5cGhgzPtsqQHyRCupLmSbYrS0dXg6/ZF1FAPyirAgMBAAGjgeIwgd8wCQYDVR0TBAIwADBQBgNVHR8ESTBHMEWgQ6BBhj9odHRwOi8vbXBraWNybC5lZ3lwdHRydXN0LmNvbS9FZ3lwdFRydXN0U2VhbGluZ0NBL0xhdGVzdENSTC5jcmwwCwYDVR0PBAQDAgbAMB0GA1UdDgQWBBSgbTpnmRnzk7m07ys9uTcWvVGzkDAfBgNVHSMEGDAWgBS15KC43nSgLTbHhRpk/f8aINUKwzARBglghkgBhvhCAQEEBAMCB4AwIAYDVR0RBBkwF4EVZXllaGlhQGVneXB0dHJ1c3QuY29tMA0GCSqGSIb3DQEBCwUAA4ICAQC7wmdpRtWiIuQsokfjUl3pruOsX7NBU46h\u002B\u002BWReQR/ceEcdzDRBVqwM7FKsTZy3/i6ACSE9MUMpMUPgtR\u002BneBq1cuknFSqhgQmnOa8mG2/nUjISNhyrcrnFSYrmJyBxT2wOO8xwtLDA2PQJIdG/n1Xn6YxwU7gbB0NApPmORhMfD1S6KINzvTj1D/EIpMaKzg7DC4wYgR2UbO8dFvNgaNtze/GRks7xQC4KMJ9udaf0JBOzyvuGtjzsB\u002B69XG0t68WVXyTIqxBZKVVU4jqG9JZdKhCHgr2P2G4nEJxTiXf3cl6iemdC1JezaoGW5FEph/wFqswiP05TVQdLOB9EkurvdrBF6sY8Xbk/2st5FvG9uAUuyQjzUETA/As4Clqr9dNirT6OVzWhI06S8CTgOONXwWTx9CjCoc\u002BERx8ce20YgVipZnKfz2MRy3bCF37\u002BCOgNyPNXy/bneFlSKEpMPYUKk5jt2z3G/I9gyozaXVGZ3sFjxHu0UX5fuiP7xknmPDdSi\u002BMwEfnh8EApgSvPlY7RLWQU8A2cUqWrCOvvuQfc2C5VG8CBP\u002BldOZt54OXSfEnx2841bTyKJGP86NvAZOTN9wFKoyhBztN9FmhG69IWbtsxdit2pbgCofh751MsN1Zbp9JerLwBxrmcEmUZfwSvE8ojOipxaszLKzg1SO7QTGCAogwggKEAgEBMFgwRDELMAkGA1UEBhMCRUcxFDASBgNVBAoTC0VneXB0IFRydXN0MR8wHQYDVQQDExZFZ3lwdCBUcnVzdCBTZWFsaW5nIENBAhAe1h3VXwXM5/GYmsL3cxj9MAsGCWCGSAFlAwQCAaCCAQUwGAYJKoZIhvcNAQkDMQsGCSqGSIb3DQEHBTAcBgkqhkiG9w0BCQUxDxcNMjAxMTAzMTAwMTQ4WjAvBgkqhkiG9w0BCQQxIgQgUt/GoPN5xkeHpV4L5olwuicaAObCbf0ORKgN4O260CIwgZkGCyqGSIb3DQEJEAIvMYGJMIGGMIGDMIGABCD6bb7asgHoS/gNVKpFneOpR/9uWobTYwah5r9IQzH\u002BcTBcMEigRjBEMQswCQYDVQQGEwJFRzEUMBIGA1UEChMLRWd5cHQgVHJ1c3QxHzAdBgNVBAMTFkVneXB0IFRydXN0IFNlYWxpbmcgQ0ECEB7WHdVfBczn8ZiawvdzGP0wCwYJKoZIhvcNAQEBBIIBAC3gpQ0ldw5TCYHG0rNMGveNtoC2vRWk7EXjPCYQJS11fkBnZ6VWAgcFtJrBHzv0x81Ik6ngvXlrl/bmB0yCm71yLcL4iBFRvB1CQ8nBlnrx24xD2OQPC\u002Bjza/7yt/y747kaJgoOcmP5Q7k92vtnIxdO\u002BX0SI3Jb9\u002BuByvJEZZTFHnjXie4gKLyR2HZqHB2VLf/scBTe2\u002BzxQx3p3Hn15Sh7Muufw0ARpZkuiT5haskusdGRF2JEsHtGX/X57JmXzHdOms/mDusbg4Mee2tLT\u002B67Bnz8FAX8qTMD8oCtOdfQaKQDhyyCsqxeLUMJ5oM28ZA/Ncf\u002BMlmVl0\u002BHKkGS13c=" } }
            };
        }
        internal static DocumentDTO ToDocumentDTO(this DocumentVM document)
        {
            return new DocumentDTO()
            {
                dateTimeIssued = document.dateTimeIssued,
                documentType = document.documentType,
                documentTypeVersion = document.documentTypeVersion,
                internalID = document.internalID,
                totalSalesAmount = Convert.ToDecimal(document.totalSalesAmount),
                netAmount = Convert.ToDecimal(document.netAmount),
                totalAmount = Convert.ToDecimal(document.totalAmount),
                totalDiscountAmount = Convert.ToDecimal(document.totalDiscountAmount),
                totalItemsDiscountAmount = Convert.ToDecimal(document.totalItemsDiscountAmount),
                extraDiscountAmount = Convert.ToDecimal(document.extraDiscountAmount),
                proformaInvoiceNumber = document.proformaInvoiceNumber,
                purchaseOrderDescription = document.purchaseOrderDescription,
                purchaseOrderReference = document.purchaseOrderReference,
                salesOrderDescription = document.salesOrderDescription,
                salesOrderReference = document.salesOrderReference,
                taxpayerActivityCode = document.taxpayerActivityCode,
                delivery = document.delivery,
                issuer = document.issuer,
                payment = document.payment,
                invoiceLines = document.invoiceLines?.Select(i => new INVOICELINESDTO()
                {
                    description = i.description,
                    internalCode = i.internalCode,
                    itemCode = i.itemCode,
                    itemsDiscount = Convert.ToDecimal(i.itemsDiscount),
                    itemType = i.itemType,
                    netTotal = Convert.ToDecimal(i.netTotal),
                    quantity = Convert.ToDecimal(i.quantity),
                    salesTotal = Convert.ToDecimal(i.salesTotal),
                    total = Convert.ToDecimal(i.total),
                    totalTaxableFees = Convert.ToDecimal(i.totalTaxableFees),
                    unitType = i.unitType,
                    valueDifference = Convert.ToDecimal(i.valueDifference),
                    taxableItems = i.taxableItems?.Select(x => new TAXABLEITEMSDTO()
                    {
                        amount = Convert.ToDecimal(x.amount),
                        rate = Convert.ToDecimal(x.rate),
                        subType = x.subType,
                        taxType = x.taxType
                    }).ToList(),
                    discount = new DISCOUNTSDTO() { amount = Convert.ToDecimal(i.discount.amount), rate = Convert.ToDecimal(i.discount.amount)},
                    unitValue = new UNITVALUESDTO() { amountEGP = Convert.ToDecimal(i.unitValue.amountEGP), amountSold = Convert.ToDecimal(i.unitValue.amountSold), currencyExchangeRate = Convert.ToDecimal(i.unitValue.currencyExchangeRate), currencySold = i.unitValue.currencySold }
                }).ToList(),
                taxTotals = document.taxTotals,
                receiver = document.receiver,
                //signatures = document.signatures
            };
        }

        internal static UserDTO ToUserDTO(this User obj)
        {
            return new UserDTO()
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                Email = obj.Email,
                LastName = obj.LastName,
                PhoneNumber = obj.PhoneNumber,
                UserName = obj.UserName,
                Title = obj.Title,
                Roles = obj.UserRoles.Select(i => new RoleDTO { Id = i.Role.Id, Name = i.Role.Name, Description = i.Role.Description }).ToList(),
                Privileges = obj.UserRoles.Select(i => i.Role).SelectMany(x => x.RolePrivileges).Select(p => new PrivilegeDTO { Id = p.Privilege.Id, Controller = p.Privilege.Controller}).ToList(),
                Permissions = obj.UserRoles.Select(i => i.Role).SelectMany(x => x.RolePrivileges).SelectMany(o => o.RolePrivilegePermissions).Select(p => new PermissionDTO { Id = p.Permission.Id, Action = p.Permission.Action}).ToList()
            };
        }
        internal static EditModelDTO ToEditModelDTO(this User obj)
        {
            return new EditModelDTO()
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                Email = obj.Email,
                LastName = obj.LastName,
                PhoneNumber = obj.PhoneNumber,
                UserName = obj.UserName,
                Title = obj.Title,
                Roles = obj.UserRoles.Select(i => i.Role.Id).ToList()
            };
        }
        internal static RoleDTO ToRoleDTO(this Role obj)
        {
            return new RoleDTO()
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description,
                Privileges  = obj.RolePrivileges.Select(o => o.Privilege.Id).ToList(),
                Permissions = obj.RolePrivileges.SelectMany(o => o.RolePrivilegePermissions).Select(p => p.Permission.Id).ToList(),
            };
        }
        internal static RoleViewModel ToRoleViewModel(this Role obj)
        {
            return new RoleViewModel()
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description,
                Privileges = AutoMapperConfiguration.Mapper.Map <List<PrivilegeDTO>> (obj.RolePrivileges.Select(o => o.Privilege).ToList()),
                Permissions = AutoMapperConfiguration.Mapper.Map <List<PermissionDTO>> (obj.RolePrivileges.SelectMany(o => o.RolePrivilegePermissions).Select(p => p.Permission).ToList())
            };
        }
    }
}
