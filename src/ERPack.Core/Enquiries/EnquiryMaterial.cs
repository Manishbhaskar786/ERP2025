using Abp.Domain.Entities.Auditing;
using ERPack.Materials;

namespace ERPack.Enquiries;

public class EnquiryMaterial : FullAuditedEntity<int>
{
    public long EnquiryId { get; set; }
    public int MaterialId { get; set; }
    public int SortOrder { get; set; }
    public virtual Material Material { get; set; }
    public virtual Enquiry Enquiry { get; set; }
}
