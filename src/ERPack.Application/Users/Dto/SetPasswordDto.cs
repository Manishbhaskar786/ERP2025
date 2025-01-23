using System.ComponentModel.DataAnnotations;

namespace ERPack.Users.Dto
{
    public class SetPasswordDto
    {
        //[Required]
        public long UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
