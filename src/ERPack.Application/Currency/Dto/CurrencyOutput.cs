using Abp.AutoMapper;

namespace ERPack.Currency.Dto
{
    [AutoMapFrom(typeof(CurrencyMaster))]
    public class CurrencyOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
