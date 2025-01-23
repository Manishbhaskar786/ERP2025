using Abp.Application.Services.Dto;

namespace ERPack.Currency.Dto
{
    public class CurrencyMasterDto : EntityDto<long>
    {
        public int? TenantId { get; set; }
        public int BusinessCurrencyId { get; set; }
        public string Name { get; set; }
    }
}
