using AutoMapper;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace eInvoicing.Signer
{
    public class AutoMapperConfiguration
    {
        public static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DocumentDTO>().ReverseMap();
                cfg.CreateMap<InvoiceLine, INVOICELINESDTO>().ReverseMap();
                cfg.CreateMap<TaxableItem, TAXABLEITEMSDTO>().ReverseMap();
                cfg.CreateMap<Error, CustomErrorDTO>().ReverseMap();
            });
            Mapper = MapperConfiguration.CreateMapper();
        }
        public static IMapper Mapper { get; private set; }
        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}
