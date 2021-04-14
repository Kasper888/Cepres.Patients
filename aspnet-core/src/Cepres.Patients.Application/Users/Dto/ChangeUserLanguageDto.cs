using System.ComponentModel.DataAnnotations;

namespace Cepres.Patients.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}