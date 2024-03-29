﻿using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface ICodeService
    {
        CreateEGSResponseDTO CreateEGSCodeUsage(CreateEGSRequestDTO obj, string Token, string URL);
        SearchEGSCodeResponseDTO SearchMyEGSCodeUsageRequests(SearchEGSCodeRequestDTO obj, string Token, string URL);
        CreateEGSResponseDTO RequestCodeReuse(RequestCodeReuseRequestDTO obj, string Token, string URL);
        SearchPublishedCodesResponseDTO SearchPublishedCodes(SearchPublishedCodesRequestDTO obj, string Token, string URL);
        GetCodeDetailsbyItemCodeResultDTO GetCodeDetailsbyItemCode(GetCodeDetailsbyItemCodeRequestDTO obj, string Token, string URL);
        GetCodeDetailsbyItemCodeResponseDTO GetCodeDetailsbyItemCodelst(GetCodeDetailsbyItemCodeRequestDTO obj, string Token, string URL);
        UpdateResponseDTO UpdateEGSCodeUsage(UpdateEGSCodeUsageRequestDTO obj, string Token, string URL);
        UpdateResponseDTO UpdateCode(UpdateCodeRequestDTO obj, string Token, string URL);
    }
}
