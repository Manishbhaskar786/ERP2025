using System.ComponentModel.DataAnnotations;

namespace ERPack.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}