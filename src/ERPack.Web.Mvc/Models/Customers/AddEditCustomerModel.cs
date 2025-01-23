using Abp.AutoMapper;
using ERPack.Categories.Dto;
using ERPack.Common.Dto;
using ERPack.Currency.Dto;
using ERPack.Customers.Dto;
using ERPack.Materials.Dto;
using ERPack.Units.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ERPack.Web.Models.Customers
{
    [AutoMap(typeof(CustomerDto))]
    public class AddEditCustomerModel
    {
        public long Id { get; set; }
        public string CustomerId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PinCode { get; set; }
        public int CountryId { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string PAN { get; set; }
        public string GSTNo { get; set; }
        public string Website { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string Industry { get; set; }
        public int CategoryId { get; set; }
        public string ReturnUrl { get; set; }
        public int BusinessCurrencyId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string CompanyLogo { get; set; }

        public IReadOnlyList<MaterialDto> Materials { get; set; }
        public IReadOnlyList<UnitOutput> Units { get; set; }
        public IReadOnlyList<CategoryOutput> Categories { get; set; }
        public IReadOnlyList<CountryMasterDto> Countries { get; set; }
        public IReadOnlyList<StateMasterDto> States { get; set; }
        public List<CurrencyOutput> BusinessCurrencies { get; set; }
        public List<CustomerMaterialPriceDto> CustomerMaterials { get; set; }
    }
}
