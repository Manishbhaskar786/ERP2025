using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ERPack.Authorization.Users;
using Microsoft.AspNetCore.Http;

namespace ERPack.Customers.Dto
{
    [AutoMapFrom(typeof(Customer))]
    public class CustomerNameOutput
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
