﻿using AutoMapper;
using DatingApp.API.Dtos;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace eInvoicing.Service.Helper
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
                cfg.CreateMap<User, LoginModelDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<User, RegistrationModelDTO>().ReverseMap();
                cfg.CreateMap<Role, RoleDTO>().ReverseMap();
                cfg.CreateMap<User, EditModelDTO>().ReverseMap();
                cfg.CreateMap<Privilege, PrivilegeDTO>().ReverseMap();
                cfg.CreateMap<Permission, PermissionDTO>().ReverseMap();
                cfg.CreateMap<Role, RoleViewModel>().ReverseMap();
            });
            Mapper = MapperConfiguration.CreateMapper();
        }
        public static IMapper Mapper { get; private set; }
        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}
