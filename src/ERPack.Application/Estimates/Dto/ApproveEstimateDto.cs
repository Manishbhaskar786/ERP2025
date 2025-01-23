using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERPack.Estimates.Dto
{
    public class ApproveEstimateDto
    {
        public long EnquiryId { get; set; }
        public bool? IsApproved { get; set; }

    }
}
