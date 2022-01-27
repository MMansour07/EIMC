using eInvoicing.DomainEntities.Base;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.DTO.Base;
using eInvoicing.Signer;
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
        internal static TDto ToDto<TDto>(this IEntity entity) where TDto : IDTO
        {
            return AutoMapperConfiguration.Mapper.Map<TDto>(entity);
        }
        internal static IList<TDto> ToDtoList<TDto>(this IEnumerable<IEntity> entity)
            where TDto : IDTO
        {
            return AutoMapperConfiguration.Mapper.Map<IList<TDto>>(entity);
        }
        internal static T As<T>(this object obj)
        {
            return AutoMapperConfiguration.Mapper.Map(obj, default(T));
        }
        internal static DocumentWithoutSignatureDTO ToDocumentWithoutSignatureDTO(this DocumentVM document)
        {
            return new DocumentWithoutSignatureDTO()
            {
                dateTimeIssued = document.dateTimeIssued.ToUniversalTime(),
                documentType = document.documentType,
                documentTypeVersion = document.documentTypeVersion,
                internalID = document.internalID,
                totalSalesAmount = Convert.ToDecimal(document.totalSalesAmount),
                netAmount = Convert.ToDecimal(document.netAmount),
                totalAmount = Convert.ToDecimal(document.totalAmount),
                totalDiscountAmount = Convert.ToDecimal(document.totalDiscountAmount),
                totalItemsDiscountAmount = Convert.ToDecimal(document.totalItemsDiscountAmount),
                extraDiscountAmount = Convert.ToDecimal(document.extraDiscountAmount),
                proformaInvoiceNumber = document.proformaInvoiceNumber ?? "",
                purchaseOrderDescription = document.purchaseOrderDescription ?? "",
                purchaseOrderReference = document.purchaseOrderReference ?? "",
                salesOrderDescription = document.salesOrderDescription ?? "",
                salesOrderReference = document.salesOrderReference ?? "",
                taxpayerActivityCode = document.taxpayerActivityCode ?? "",
                delivery = new DELIVERIESDTO()
                {
                    approach = document.delivery.approach ?? "",
                    countryOfOrigin = document.delivery.countryOfOrigin ?? "NA",
                    dateValidity = "",
                    exportPort = document.delivery.exportPort ?? "",
                    grossWeight = document.delivery.grossWeight ,
                    netWeight = document.delivery.netWeight,
                    packaging = document.delivery.packaging ?? "",
                    terms = document.delivery.terms ?? ""
                },
                issuer = new ISSUERSDTO()
                { 
                    address = new ISSUERADDESSESDTO()
                    {
                        additionalInformation = document.issuer.address.additionalInformation ?? "",
                        branchID = document.issuer.address.branchID ,
                        buildingNumber = document.issuer.address.buildingNumber ?? "",
                        country = document.issuer.address.country ?? "",
                        floor = document.issuer.address.floor ?? "NA",
                        governate = document.issuer.address.governate ?? "",
                        landmark = document.issuer.address.landmark ?? "NA",
                        postalCode = document.issuer.address.postalCode ?? "NA",
                        regionCity = document.issuer.address.regionCity ??"NA",
                        room = document.issuer.address.room ?? "NA",
                        street = document.issuer.address.street ?? "NA"
                    },
                    id = document.issuer.id ,
                    name = document.issuer.name ,
                    type = document.issuer.type
                    
                },
                payment = new PAYMENTSDTO()
                { 
                    bankAccountIBAN = document.payment.bankAccountIBAN ?? "",
                    bankAccountNo = document.payment.bankAccountNo ?? "",
                    bankAddress = document.payment.bankAddress ?? "",
                    bankName = document.payment.bankName ?? "" ,
                    swiftCode = document.payment.swiftCode ?? "",
                    terms = document.payment.terms ?? ""
                },
                invoiceLines = document.invoiceLines?.Select(i => new INVOICELINESDTO()
                {
                    description = i.description ?? "",
                    internalCode = i.internalCode,
                    itemCode = i.itemCode,
                    itemsDiscount = Convert.ToDecimal(i.itemsDiscount),
                    itemType = i.itemType ?? "" ,
                    netTotal = Convert.ToDecimal(i.netTotal),
                    quantity = Convert.ToDecimal(i.quantity),
                    salesTotal = Convert.ToDecimal(i.salesTotal),
                    total = Convert.ToDecimal(i.total),
                    totalTaxableFees = Convert.ToDecimal(i.totalTaxableFees),
                    unitType = i.unitType ?? "" ,
                    valueDifference = Convert.ToDecimal(i.valueDifference),
                    taxableItems = i.taxableItems?.Select(x => new TAXABLEITEMSDTO()
                    {
                        amount = Convert.ToDecimal(x.amount),
                        rate = Convert.ToDecimal(x.rate),
                        subType = x.subType ?? "",
                        taxType = x.taxType ?? "" 
                    }).ToList(),
                    discount = new DISCOUNTSDTO() { amount = Convert.ToDecimal(i.discount.amount), rate = Convert.ToDecimal(i.discount.amount)},
                    unitValue = new UNITVALUESDTO() { amountEGP = Convert.ToDecimal(i.unitValue.amountEGP), amountSold = Convert.ToDecimal(i.unitValue.amountSold), currencyExchangeRate = Convert.ToDecimal(i.unitValue.currencyExchangeRate), currencySold = i.unitValue.currencySold }
                }).ToList(),
                taxTotals = document.taxTotals,
                receiver = new RECEIVERSDTO()
                { 
                    address = new RECEIVERADDRESSESDTO()
                    { 
                        additionalInformation = document.receiver.address.additionalInformation ?? "",
                        buildingNumber = document.receiver.address.buildingNumber ?? "NA",
                        country = document.receiver.address.country ?? "" ,
                        floor = document.receiver.address.floor?? "",
                        governate = document.receiver.address.governate ?? "",
                        landmark = document.receiver.address.landmark ?? "",
                        postalCode = document.receiver.address.postalCode ?? "",
                        regionCity = document.receiver.address.regionCity ?? "NA",
                        room = document.receiver.address.room ?? "",
                        street = document.receiver.address.street ?? "NA"
                    },
                   id =   document.receiver.id ?? "",
                   name = document.receiver.name,
                   type = document.receiver.type
                },
               //references = new List<string>() { document.parentId}
            };
        }
    }
}
